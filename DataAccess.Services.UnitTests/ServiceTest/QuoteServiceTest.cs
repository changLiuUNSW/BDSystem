using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DataAccess.Common;
using DataAccess.EntityFramework;
using DataAccess.EntityFramework.Infrastructure;
using DataAccess.EntityFramework.Models.Quad;
using DataAccess.EntityFramework.Models.Quote;
using DataAccess.EntityFramework.TypeLibrary;
using DateAccess.Services.MailService;
using DateAccess.Services.QuoteService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DataAccess.Services.UnitTests.ServiceTest
{
    [TestClass]
    public class QuoteServiceTest
    {
        private Mock<ApplicationSettings> _applicationSettings;
        private Mock<IEmailHelper> _emailHelper;
        private Mock<IQuoteEmailService> _quoteEmailService;
        private Mock<QuoteService> _quoteService;
        private Mock<IUnitOfWork> _unitOfWork;

        [ExpectedException(typeof (ArgumentException))]
        [TestMethod]
        public void IfQuoteNotFoundThrowArgumentException()
        {
            _quoteService.Setup(l => l.GetByKey(It.Is<int>(k => k == 1), false)).Returns((Quote) null);
            _quoteService.Object.CancelQuote(1, "chang");
        }


        [TestMethod]
        public void WhenCancelQuote_InsertToHistoryTableAndChangeStatusToCancel()
        {
            var quote = new Quote
            {
                StatusId = 1,
                Id = 1
            };
            QuoteHistory history = null;
            _quoteService.Setup(l => l.GetByKey(It.Is<int>(k => k == 1), false)).Returns(quote);
            _unitOfWork.Setup(l => l.Save()).Returns(1);
            _unitOfWork.Setup(l => l.GetRepository<QuoteHistory>().
                Add(It.IsAny<QuoteHistory>())).Callback<QuoteHistory>(p => history = p);
            _quoteService.Object.CancelQuote(1, "chang");


            Assert.AreEqual(history.QuoteId, quote.Id);
            Assert.AreEqual(history.ToStatusId, (int) QuoteStatusTypes.Cancel);
            Assert.AreEqual(history.User, "chang");
            Assert.AreEqual(history.FromStatusId, 1);
            Assert.AreEqual(history.Description, "Quote cancelled");
        }

        [TestMethod]
        public void WhenQuoteContactMade_AllQuestionsResultForNotCalledWillRemoved()
        {
            var quoteQuestionResults = new List<QuoteQuestionResult>();
            var model = new QuotePostModel
            {
                QuoteId = 1,
                Date = DateTime.Now
            };
            var quote = new Quote
            {
                StatusId = 1,
                Id = 1
            };
            _quoteService.Setup(l => l.GetByKey(It.Is<int>(k => k == 1), false)).Returns(quote);
            _unitOfWork.Setup(l => l.Save()).Returns(1);
            var repo = new Mock<IRepository<QuoteQuestionResult>>();

            repo.Setup(l => l.Get(It.IsAny<Expression<Func<QuoteQuestionResult, bool>>>()))
                .Returns(quoteQuestionResults);
            repo.Setup(l => l.RemoveRange(It.IsAny<IList<QuoteQuestionResult>>()));
            _unitOfWork.Setup(
                l => l.GetRepository<QuoteQuestionResult>())
                .Returns(repo.Object);
            _quoteService.Object.Contact(model);
            repo.Verify(l => l.RemoveRange(quoteQuestionResults), Times.Once);
            Assert.AreEqual(quote.ContactCheckOverDue, false);
        }

        [TestMethod]
        public void Success_WhenCurrentStatus_Is_WPReview_And_LeadPerson_Is_BD_NextStatus_Is_BDReview()
        {
            var quote = new Quote
            {
                StatusId = (int) QuoteStatusTypes.WPReview
            };
            _unitOfWork.Setup(
                l => l.GetRepository<QuadPhoneBook>().Any(It.IsAny<Expression<Func<QuadPhoneBook, bool>>>()))
                .Returns(true);
            Assert.AreEqual(_quoteService.Object.NextStatus(quote, true), QuoteStatusTypes.BDReview);
        }

        [TestMethod]
        public void Success_WhenCurrentStatus_Is_WPReview_And_LeadPerson_Is_Not_BD_NextStatus_Is_QPReview()
        {
            var quote = new Quote
            {
                StatusId = (int) QuoteStatusTypes.WPReview
            };
            _unitOfWork.Setup(
                l => l.GetRepository<QuadPhoneBook>().Any(It.IsAny<Expression<Func<QuadPhoneBook, bool>>>()))
                .Returns(false);
            Assert.AreEqual(_quoteService.Object.NextStatus(quote, true), QuoteStatusTypes.QPReview);
        }

        [TestMethod]
        public void Success_WhenCurrentStatus_Is_Print_And_ToCurrentAnswers_Exist_NextStatus_Is_Current_And_WasCurrentFlag_Is_true()
        {
            var quote = new Quote
            {
                StatusId = (int) QuoteStatusTypes.Print,
                QuestionResults = new List<QuoteQuestionResult>
                {
                    new QuoteQuestionResult
                    {
                        Question = new QuoteQuestion
                        {
                            Type = QuoteQuestionType.ToCurrent
                        }
                    }
                }
            };
            Assert.AreEqual(_quoteService.Object.NextStatus(quote, true), QuoteStatusTypes.Current);
            Assert.AreEqual(quote.WasCurrent, true);
        }


        [TestMethod]
        public void Success_WhenCurrentStatus_Is_Print_And_ToCurrentAnswers_Not_Exist_NextStatus_Is_PresentToClient()
        {
            var quote = new Quote
            {
                StatusId = (int) QuoteStatusTypes.Print,
                QuestionResults = new List<QuoteQuestionResult>()
            };
            Assert.AreEqual(_quoteService.Object.NextStatus(quote, true), QuoteStatusTypes.PresentToClient);
        }

        [TestMethod]
        public void Success_WhenCurrentStatus_Is_WPIssues_And_Issue_NextStatus_Exists_NextStatus_Is_Equal_With_It()
        {
            var quote = new Quote
            {
                StatusId = (int) QuoteStatusTypes.WPIssues,
                QuoteIssues = new List<QuoteIssue>
                {
                    new QuoteIssue
                    {
                        Id = 1,
                        NextStatusId = (int) QuoteStatusTypes.PresentToClient
                    }
                }
            };
            Assert.AreEqual(_quoteService.Object.NextStatus(quote, true, 1), QuoteStatusTypes.PresentToClient);
        }


        [TestMethod]
        public void Success_WhenCurrentStatus_Is_WPIssues_And_Issue_NextStatus_Not_Exists_NextStatus_Is_Null()
        {
            var quote = new Quote
            {
                StatusId = (int)QuoteStatusTypes.WPIssues,
                QuoteIssues = new List<QuoteIssue>
                {
                    new QuoteIssue
                    {
                        Id = 2,
                        NextStatusId = (int) QuoteStatusTypes.PresentToClient
                    }
                }
            };
            Assert.AreEqual(_quoteService.Object.NextStatus(quote, true, 1), null);
        }



        [TestMethod]
        public void Success_WhenCurrentStatus_Is_QPIssues_And_Issue_NextStatus_Exists_NextStatus_Is_Equal_With_It()
        {
            var quote = new Quote
            {
                StatusId = (int)QuoteStatusTypes.QPIssues,
                QuoteIssues = new List<QuoteIssue>
                {
                    new QuoteIssue
                    {
                        Id = 1,
                        NextStatusId = (int) QuoteStatusTypes.PresentToClient
                    }
                }
            };
            Assert.AreEqual(_quoteService.Object.NextStatus(quote, true, 1), QuoteStatusTypes.PresentToClient);
        }

       



        [TestMethod]
        public void Success_WhenCurrentStatus_Is_QPIssues_And_Issue_NextStatus_Not_Exists_NextStatus_Is_Null()
        {
            var quote = new Quote
            {
                StatusId = (int)QuoteStatusTypes.QPIssues,
                QuoteIssues = new List<QuoteIssue>
                {
                    new QuoteIssue
                    {
                        Id = 2,
                        NextStatusId = (int) QuoteStatusTypes.PresentToClient
                    }
                }
            };
            Assert.AreEqual(_quoteService.Object.NextStatus(quote, true, 1), null);
        }

        [TestMethod]
        public void Failed_WhenCurrentStatus_IS_WPReview_NextStatus_IS_QPIssues()
        {
            var quote = new Quote
            {
                StatusId = (int)QuoteStatusTypes.WPReview,
            };
            Assert.AreEqual(_quoteService.Object.NextStatus(quote, false), QuoteStatusTypes.QPIssues);
        }

        [TestMethod]
        public void Failed_WhenCurrentStatus_IS_NOT_WPReview_NextStatus_IS_WPIssues()
        {
            var quote = new Quote
            {
                StatusId = (int)QuoteStatusTypes.BDReview,
            };
            Assert.AreEqual(_quoteService.Object.NextStatus(quote, false), QuoteStatusTypes.WPIssues);
        }


      





        [TestMethod]
        public void Sccuess_NextStatusNormalTest()
        {
            //New to Estimation
            Assert.AreEqual(_quoteService.Object.NextStatus(new Quote
            {
                StatusId = (int) QuoteStatusTypes.New
            }, true), QuoteStatusTypes.Estimation);
            //Estimation to WPReview
            Assert.AreEqual(_quoteService.Object.NextStatus(new Quote
            {
                StatusId = (int) QuoteStatusTypes.Estimation
            }, true), QuoteStatusTypes.WPReview);
            //QPReview to PreFinalReview
            Assert.AreEqual(_quoteService.Object.NextStatus(new Quote
            {
                StatusId = (int) QuoteStatusTypes.QPReview
            }, true), QuoteStatusTypes.PreFinalReview);
            //BDReview to PreFinalReview
            Assert.AreEqual(_quoteService.Object.NextStatus(new Quote
            {
                StatusId = (int) QuoteStatusTypes.BDReview
            }, true), QuoteStatusTypes.PreFinalReview);
            //PreFinalReview to FinalReview
            Assert.AreEqual(_quoteService.Object.NextStatus(new Quote
            {
                StatusId = (int) QuoteStatusTypes.PreFinalReview
            }, true), QuoteStatusTypes.FinalReview);

            //PresentToClient to current

            Assert.AreEqual(_quoteService.Object.NextStatus(new Quote
            {
                StatusId = (int) QuoteStatusTypes.PresentToClient
            }, true), QuoteStatusTypes.Current);
        }


        [TestInitialize]
        public void Init()
        {
            _quoteEmailService = new Mock<IQuoteEmailService>();
            _emailHelper = new Mock<IEmailHelper>();
            _applicationSettings = new Mock<ApplicationSettings>();
            _unitOfWork = new Mock<IUnitOfWork>();
            _quoteService = new Mock<QuoteService>(_unitOfWork.Object, _quoteEmailService.Object, _emailHelper.Object,
                _applicationSettings.Object);
        }
    }
}