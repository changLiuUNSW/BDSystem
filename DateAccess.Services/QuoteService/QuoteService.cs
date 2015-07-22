using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Common;
using DataAccess.Common.Util;
using DataAccess.EntityFramework;
using DataAccess.EntityFramework.Models.Quad;
using DataAccess.EntityFramework.Models.Quote;
using DataAccess.EntityFramework.Repositories;
using DataAccess.EntityFramework.TypeLibrary;
using DateAccess.Services.MailService;

namespace DateAccess.Services.QuoteService
{
    public class QuotePostModel
    {
        public int QuoteId { get; set; }
        public string Url { get; set; }
        public DateTime? Date { get; set; }
    }

    public class ReviewFailedModel
    {
        public bool PricePageOnly { get; set; }
        public bool IsPdf { get; set; }
        public decimal? NewPa { get; set; }
        public int QuoteId { get; set; }
        public string Url { get; set; }
        public string IssueDetail { get; set; }
    }

    public class ResolveModel
    {
        public int IssueId { get; set; }
        public string Url { get; set; }
        public string Comment { get; set; }
    }

    public class QuestionResultModel
    {
        public int QuestionId { get; set; }
        public int? AnswerId { get; set; }
        public string Additional { get; set; }
    }

    public interface IQuoteService:IRepositoryService<Quote>
    {
        Quote UpdateStatusAndHistory(Quote quote, string user, string description, int nextStatusId);
        QuoteStatusTypes? NextStatus(Quote quote, bool success, int? issueId = null);
        Quote CancelQuote(int quoteId, string user);
        Quote Contact(QuotePostModel model);
        Quote NotSendEmail(string user, int quoteId, List<QuestionResultModel> questionResults);
        Quote NotContact(string user, int quoteId, List<QuestionResultModel> questionResults);
        Quote Finalize(string user, int quoteId, List<QuestionResultModel> questionResults);
        Quote Dead(string user, int quoteId, List<QuestionResultModel> questionResults);
        Quote NotDead(string user, int quoteId, List<QuestionResultModel> questionResults);
        Quote NotAdjust(string user, int quoteId, List<QuestionResultModel> questionResults);
        Quote ReUploadQuote(QuotePostModel model,FileInfo tempFile, string originalFileName);
        Task<Quote> Reviewfailed(string user, ReviewFailedModel model);
        Task<int> Resolved(string user, ResolveModel model);
        Task<int> ResolveWithUpload(string user, ResolveModel model, FileInfo tempFile, string originalFileName);
        Task<Quote> UploadQuote(QuotePostModel model, FileInfo tempFile, string originalFileName, string user);
        Task<Quote> BdReviewPass(QuotePostModel model, string user, bool isBdgm);
        Task<Quote> QpReviewPass(QuotePostModel model, string user);
        Task<Quote> PreFinalReviewPass(QuotePostModel model, string user);
        Task<Quote> FinalReviewPass(QuotePostModel model, string user);
        Task<Quote> Print(QuotePostModel model, string user);
        Task<Quote> PriceCheckfailed(string user, ReviewFailedModel model);
        Task<Quote> Adjust(string user, ReviewFailedModel model);
        Task<List<QuoteStatus>> GetAllStatus();
        Task<List<OverdueModel>> GetOverDueList();
        List<QuoteResultModel> GetQuoteResultListByType(int quoteId, QuoteQuestionType? type);
        Task<Quote> SendToWp(QuotePostModel model, string user);
        IList<QuoteQuestion> GetQuestionsByType(int questionType);
    }

    internal class QuoteService : RepositoryService<Quote>, IQuoteService
    {
        private readonly IQuoteEmailService _quoteEmailService;
        private readonly IEmailHelper _emailHelper;
        private readonly ApplicationSettings _settings;
        public QuoteService(IUnitOfWork unitOfWork, IQuoteEmailService quoteEmailService, IEmailHelper emailHelper, ApplicationSettings settings)
            : base(unitOfWork)
        {
            _quoteEmailService = quoteEmailService;
            _emailHelper = emailHelper;
            _settings = settings;
        }

        public async Task<Quote> Adjust(string user, ReviewFailedModel model)
        {
            int issuenextStatusId;
            var quote = GetByKey(model.QuoteId);
            if (quote == null) throw new ArgumentException("invalid Quote Id " + model.QuoteId);
            quote.LastestPA = model.NewPa == null ? quote.LastestPA : model.NewPa.Value;
            quote.LastAdjustCheckDate = DateTime.Now;
            quote.AjustCheckOverDue = false;
            if (model.IsPdf)
            {
                issuenextStatusId = quote.StatusId;
                UpdateStatusAndHistory(quote, user, model.IssueDetail, (int)QuoteStatusTypes.WPIssues);
            }
            else
            {
                var issueNextStatusType = NextIssueStatus(quote);
                var nextstatus = NextStatus(quote, false);
                if (issueNextStatusType == null) throw new ArgumentException("Cannot find corresponding next status for " + quote.Status.Name);
                if (nextstatus == null) throw new ArgumentException("Cannot find corresponding next status for " + quote.Status.Name);
                issuenextStatusId = (int)issueNextStatusType.Value;
                UpdateStatusAndHistory(quote, user, model.IssueDetail, (int)nextstatus);
            }
            //Add quote Issue
            var quoteIssue = new QuoteIssue
            {
                QuoteId = quote.Id,
                Resolved = false,
                NextStatusId = issuenextStatusId,
                CreatedBy = user,
                UploadRequired = true,
                IsEmailOnly = model.IsPdf,
                UploadPricePageOnly = model.PricePageOnly,
                IssueDetail = model.IssueDetail,
                CreatedDate = DateTime.Now
            };
            AddQuoteIssue(quoteIssue, quote);
            try
            {
                var toList = new List<string> { _settings.WPEmail };
                await _quoteEmailService.SendIssueAlert(quote, model.IssueDetail, model.Url, toList);
            }
            catch (Exception e) { }
            Save();
            return quote;
        }

        public async Task<List<QuoteStatus>> GetAllStatus()
        {
            var statusList = await UnitOfWork.GetRepository<QuoteStatus>().GetAsync();
            return statusList;
        }

        public async Task<List<OverdueModel>> GetOverDueList()
        {
            var overdueList = await UnitOfWork.QuoteRepository.GetOverDueList();
            return overdueList;
        }

        public List<QuoteResultModel> GetQuoteResultListByType(int quoteId, QuoteQuestionType? type)
        {
            return UnitOfWork.QuoteRepository.GetQuoteResultListByType(quoteId, type);
        }

        public async Task<Quote> SendToWp(QuotePostModel model, string user)
        {
            const string description = "Cost estimations are generated and sent to WP";
            var quote = GetByKey(model.QuoteId);
            if (quote == null) throw new ArgumentException("Invalid Quote Id " + model.QuoteId);
            var nextstatus = NextStatus(quote, true);
            if (nextstatus == null) throw new ArgumentException("Cannot find corresponding next status for " + quote.Status.Name);
            //TODO: add handler for system cost
            if (quote.QuoteCost.Any(l => l.CostType == CostType.System)) { throw  new NotImplementedException();}
            UpdateStatusAndHistory(quote, user,description,(int)nextstatus.Value);
            try
            {
                await _quoteEmailService.SendCostsToWp(quote, model.Url);
            }
            catch (Exception e)
            {

            }

            Save();
            return quote;
        }

        public Quote UpdateStatusAndHistory(Quote quote, string user, string description, int nextStatusId)
        {
            quote.LastUpdateDate = DateTime.Now;
            UnitOfWork.GetRepository<QuoteHistory>().Add(new QuoteHistory
            {
                Description = description,
                Time = DateTime.Now,
                User = user,
                FromStatusId = quote.StatusId,
                ToStatusId = nextStatusId,
                QuoteId = quote.Id
            });
            quote.StatusId = nextStatusId;
            //Update corresponding lead status to quoted
            if (quote.Lead != null &&
                quote.StatusId != (int)QuoteStatusTypes.New &&
                quote.StatusId != (int)QuoteStatusTypes.Estimation &&
                quote.StatusId != (int)QuoteStatusTypes.Cancel)
            {
                quote.Lead.LeadStatusId = (int)LeadStatusTypes.Quoted;
            }
            return quote;
        }

       

        public QuoteStatusTypes? NextStatus(Quote quote,bool success, int? issueId = null)
        {
            QuoteStatusTypes? nextStatus = null;
            var currentStatus = quote.StatusId;
            if (success)
            {
                switch (currentStatus)
                {
                    case (int)QuoteStatusTypes.New:
                        nextStatus = QuoteStatusTypes.Estimation;
                        break;
                    case (int)QuoteStatusTypes.Estimation:
                        nextStatus = QuoteStatusTypes.WPReview;
                        break;
                    case (int)QuoteStatusTypes.WPReview:
                        //If lead personal is BD, it required BDGM and BD to review parallelly.
                        nextStatus = UnitOfWork.GetRepository<QuadPhoneBook>()
                            .Any(l => l.Intial == quote.LeadPersonal.Initial && l.Group == "BD") ? QuoteStatusTypes.BDReview : QuoteStatusTypes.QPReview;
                        break;
                    case (int)QuoteStatusTypes.QPReview:
                        nextStatus = QuoteStatusTypes.PreFinalReview;
                        break;
                    case (int)QuoteStatusTypes.BDReview:
                        nextStatus = QuoteStatusTypes.PreFinalReview;
                        break;
                    case (int)QuoteStatusTypes.PreFinalReview:
                        nextStatus = QuoteStatusTypes.FinalReview;
                        break;
                    case (int)QuoteStatusTypes.FinalReview:
                        nextStatus = QuoteStatusTypes.Print;
                        break;
                    case (int)QuoteStatusTypes.Print:
                        nextStatus = quote.QuestionResults.Any(l => l.Question.Type == QuoteQuestionType.ToCurrent) ? 
                            QuoteStatusTypes.Current : QuoteStatusTypes.PresentToClient;
                        break;
                    case (int)QuoteStatusTypes.PresentToClient:
                        nextStatus = QuoteStatusTypes.Current;
                        break;
                    case (int)QuoteStatusTypes.WPIssues:
                        var wplastIssue = quote.QuoteIssues.SingleOrDefault(l => l.Id == issueId);
                        if (wplastIssue != null) nextStatus = (QuoteStatusTypes)wplastIssue.NextStatusId;
                        break;
                    case (int)QuoteStatusTypes.QPIssues:
                        var qplastIssue = quote.QuoteIssues.SingleOrDefault(l => l.Id == issueId);
                        if (qplastIssue != null) nextStatus = (QuoteStatusTypes)qplastIssue.NextStatusId;
                        break;
                }
            }
            else if (quote.StatusId == (int)QuoteStatusTypes.WPReview)
            {
                nextStatus = QuoteStatusTypes.QPIssues;
            }
            else
            {
                nextStatus = QuoteStatusTypes.WPIssues;
            }


            if (nextStatus == QuoteStatusTypes.Current)
            {
                quote.WasCurrent = true;
            }
            
            return nextStatus;
        }

       
        public async Task<Quote> PriceCheckfailed(string user, ReviewFailedModel model)
        {
            int issuenextStatusId;
            var quote = GetByKey(model.QuoteId);
            if (quote == null) throw new ArgumentException("invalid Quote Id " + model.QuoteId);
            quote.LastestPA = model.NewPa == null ? quote.LastestPA : model.NewPa.Value;
            if (model.IsPdf )
            {
                issuenextStatusId = quote.StatusId;
                UpdateStatusAndHistory(quote, user, model.IssueDetail, (int)QuoteStatusTypes.WPIssues);
            }
            else
            {
                var issueNextStatusType = NextIssueStatus(quote);
                var nextstatus = NextStatus(quote, false);
                if (issueNextStatusType == null) throw new ArgumentException("Cannot find corresponding next status for " + quote.Status.Name);
                if (nextstatus == null) throw new ArgumentException("Cannot find corresponding next status for " + quote.Status.Name);
                issuenextStatusId = (int)issueNextStatusType.Value;
                UpdateStatusAndHistory(quote, user, model.IssueDetail,(int) nextstatus);
            }
            //Add quote Issue
            var quoteIssue = new QuoteIssue
            {
                QuoteId = quote.Id,
                Resolved = false,
                NextStatusId = issuenextStatusId,
                CreatedBy = user,
                UploadRequired = true,
                IsEmailOnly = model.IsPdf,
                UploadPricePageOnly=model.PricePageOnly,
                IssueDetail = model.IssueDetail,
                CreatedDate = DateTime.Now
            };
            AddQuoteIssue(quoteIssue,quote);
            try
            {
                var toList = new List<string> { _settings.WPEmail };
                await _quoteEmailService.SendIssueAlert(quote, model.IssueDetail, model.Url, toList);
            }
            catch (Exception e){}
            Save();
            return quote;
        }

        public Quote Dead(string user, int quoteId, List<QuestionResultModel> questionResults)
        {
            const string description = "Quote is dead";
            var quote = GetByKey(quoteId);
            if(quote==null) throw new ArgumentException("Invalid Quote Id "+quoteId);
            //Save Dead results
            UpdateQuestionResult(questionResults, quoteId, user, QuoteQuestionType.Dead);
            UnitOfWork.GetRepository<QuoteHistory>().Add(new QuoteHistory
            {
                Description = description,
                Time = DateTime.Now,
                User = user,
                FromStatusId = quote.StatusId,
                ToStatusId = (int)QuoteStatusTypes.Dead,
                QuoteId = quote.Id
            });
            quote.LastDeadCheckDate = DateTime.Now;
            quote.LastUpdateDate = DateTime.Now;
            quote.StatusId =(int) QuoteStatusTypes.Dead;
            quote.DeadCheckOverDue = false;
            Save();
            return quote;
        }

        public Quote NotDead(string user, int quoteId, List<QuestionResultModel> questionResults)
        {
            var quote = GetByKey(quoteId);
            if (quote == null) throw new ArgumentException("Invalid Quote Id " + quoteId);
            UpdateQuestionResult(questionResults, quoteId, user, QuoteQuestionType.NoDead);
            quote.LastDeadCheckDate = DateTime.Now;
            quote.LastUpdateDate = DateTime.Now;
            quote.DeadCheckOverDue = false;
            Save();
            return quote;
        }

        public Quote NotAdjust(string user, int quoteId, List<QuestionResultModel> questionResults)
        {
            var quote = GetByKey(quoteId);
            if (quote == null) throw new ArgumentException("Invalid Quote Id " + quoteId);
            UpdateQuestionResult(questionResults, quoteId, user, QuoteQuestionType.NoAdjust);
            quote.LastAdjustCheckDate = DateTime.Now;
            quote.LastUpdateDate = DateTime.Now;
            quote.AjustCheckOverDue = false;
            Save();
            return quote;
        }

        public Quote ReUploadQuote(QuotePostModel model, FileInfo tempFile, string originalFileName)
        {
            var quote = GetByKey(model.QuoteId);
            if (quote == null) throw new ArgumentException("Invalid Quote Id " + model.QuoteId);
            UploadQuoteDocumment(tempFile, originalFileName, quote);
            Save();
            return quote;
        }

        public async Task<Quote> Reviewfailed(string user, ReviewFailedModel model)
        {
            var sendToWp = true;
            var quote = GetByKey(model.QuoteId);
            var nextIssueStatus = NextIssueStatus(quote);
            if (quote == null) throw new ArgumentException("invalid Quote Id " + model.QuoteId);
            var nextstatus = NextStatus(quote, false);
            if (nextstatus == null) throw new ArgumentException("Cannot find corresponding next status for " + quote.Status.Name);
            if (nextIssueStatus == null) throw new ArgumentException("Cannot find corresponding next status for " + quote.Status.Name);
            //If status is WP Review, we need to change all the costs to the draft
            if (quote.StatusId == (int) QuoteStatusTypes.WPReview)
            {
                foreach (var cost in quote.QuoteCost)
                {
                    cost.Status = CostStatus.Draft;
                }
                //Send back to QP
                sendToWp = false;
            }
            //If Status is BD Review, we reset all status for BD and BDGM.
            if (quote.StatusId == (int) QuoteStatusTypes.BDReview)
            {
                quote.BDReview = false;
                quote.BDGMReview = false;
            }
            //Add quote Issue
            var quoteIssue = new QuoteIssue
            {
                QuoteId = quote.Id,
                Resolved = false,
                NextStatusId = (int)nextIssueStatus.Value,
                CreatedBy = user,
                IssueDetail = model.IssueDetail,
                CreatedDate = DateTime.Now
            };
            AddQuoteIssue(quoteIssue, quote);
            UpdateStatusAndHistory(quote, user, model.IssueDetail,(int) nextstatus);
            //WP needs to re upload new merged quote document
            if (quote.StatusId == (int)QuoteStatusTypes.WPIssues)
            {
                quoteIssue.UploadRequired = true;
            }
            Save();
            //Send Issue Alert
            var toList = new List<string>
            {
                sendToWp ? _settings.WPEmail : _emailHelper.GetEmailByInitial(quote.LeadPersonal.Initial)
            };
            try
            {
                await _quoteEmailService.SendIssueAlert(quote, model.IssueDetail, model.Url, toList);
            }
            catch (Exception e)
            {
                
            }
            return quote;
        }

        public async Task<int> Resolved(string user, ResolveModel model)
        {
            var issue= ResolveIssue(user, model);
            var result = Save();
            //If next Status is WPReview
            if (issue.NextStatusId == (int)QuoteStatusTypes.WPReview)
            {
                try
                {
                    await _quoteEmailService.SendCostsToWp(issue.Quote, model.Url);
                }
                catch
                {
                }
            }
            return result;
        }



        public async Task<int> ResolveWithUpload(string user, ResolveModel model, FileInfo tempFile, string originalFileName)
        {
            var issue = ResolveIssue(user, model);
            if (issue.UploadPricePageOnly)
            {
                UploadQuotePricePage(tempFile, originalFileName, issue.Quote);
         
            }
            else
            {
                UploadQuoteDocumment(tempFile, originalFileName, issue.Quote);  
            }

            if (issue.IsEmailOnly)await _quoteEmailService.SendQuoteDocumentToQp(issue.Quote, model.Url, issue.UploadPricePageOnly);
           
            var result = Save();
            if (issue.NextStatusId != (int) QuoteStatusTypes.WPReview) return result;
            try
            {
                await _quoteEmailService.SendCostsToWp(issue.Quote, model.Url);
            }
            catch
            {
            }
            //TODO: if nextstatus = print, we need to send reprint reminder
            return result;
        }

        public async Task<Quote> UploadQuote(QuotePostModel model, FileInfo tempFile, string originalFileName, string user)
        {
            const string description = "Upload merged quote document";
            var toList = new List<string>();
            var quote = GetByKey(model.QuoteId);
            if(quote==null) throw new ArgumentException("Invalid Quote Id "+model.QuoteId);
            var nextstatus = NextStatus(quote, true);
            if (nextstatus == null) throw new ArgumentException("Cannot find corresponding next status for " + quote.Status.Name);
            UpdateStatusAndHistory(quote, user, description, (int)nextstatus);
            UploadQuoteDocumment(tempFile, originalFileName, quote);
            if (quote.StatusId == (int) QuoteStatusTypes.BDReview)
            {
                var bdgmEmail = _settings.BDGMEmail;
                toList.Add(bdgmEmail);
            }
            var qpEmail = _emailHelper.GetEmailByInitial(quote.LeadPersonal.Initial);
            toList.Add(qpEmail);
            try
            {
                await _quoteEmailService.SendMergedQuote(quote, model.Url, toList);
            }
            catch (Exception)
            {
                
               
            }
            Save(); 
            return quote;
        }

     

        public async Task<Quote> BdReviewPass(QuotePostModel model, string user,bool isBdgm)
        {
         
            var description = isBdgm ? "Quote is reviewed by BDGM" : "Quote is reviewed by BD";
            var quote = GetByKey(model.QuoteId);
            if(quote==null) throw new ArgumentException("Invalid Quote Id "+model.QuoteId);
            var nextstatus = NextStatus(quote, true);
            if (nextstatus == null) throw new ArgumentException("Cannot find corresponding next status for " + quote.Status.Name);
            if (isBdgm)
            {
                quote.BDGMReview = true;
            }
            else
            {
                quote.BDReview = true;
            }
            //If both BD and GM review the quote successfully, change quote to the next status and send Email to AY/DJN for Final Review
            if (quote.BDGMReview && quote.BDReview)
            {
              
                UpdateStatusAndHistory(quote, user, description,(int)nextstatus);
                Save();
              
            }
            else
            {
                quote.LastUpdateDate = DateTime.Now;
                UnitOfWork.GetRepository<QuoteHistory>().Add(new QuoteHistory
                {
                    Description = description,
                    Time = DateTime.Now,
                    User = user,
                    FromStatusId = quote.StatusId,
                    ToStatusId = quote.StatusId,
                    QuoteId = quote.Id
                });
                Save();
            }
            return quote;
        }

        public async Task<Quote> QpReviewPass(QuotePostModel model, string user)
        {
            const string description = "Quote is reviewed by QP";
            var quote = GetByKey(model.QuoteId);
            if(quote==null) throw new ArgumentException("Invalid Quote Id "+model.QuoteId);
            var nextstatus = NextStatus(quote, true);
            if (nextstatus == null) throw new ArgumentException("Cannot find corresponding next status for " + quote.Status.Name);
            UpdateStatusAndHistory(quote, user, description, (int)nextstatus);
            return quote;
        }

        public async Task<Quote> PreFinalReviewPass(QuotePostModel model, string user)
        {
            var toList = new List<string>();
            const string description = "Quote is pre-final reviewed";
            var quote = GetByKey(model.QuoteId);
            if (quote == null) throw new ArgumentException("Invalid Quote Id " + model.QuoteId);
            var nextstatus = NextStatus(quote, true);
            if (nextstatus == null) throw new ArgumentException("Cannot find corresponding next status for " + quote.Status.Name);
            UpdateStatusAndHistory(quote, user, description, (int)nextstatus);
            try
            {
                toList.Add(_settings.QuoteFinalReviewEmail);
                await _quoteEmailService.SendMergedQuote(quote, model.Url, toList);
            }
            catch (Exception e) { }
            Save();
            return quote;
        }

        public async Task<Quote> FinalReviewPass(QuotePostModel model, string user)
        {
            const string description = "Quote is final reviewed. Print whole quote next";
            var quote = GetByKey(model.QuoteId);
            if (quote == null) throw new ArgumentException("Invalid Quote Id " + model.QuoteId);
            var nextstatus = NextStatus(quote, true);
            if (nextstatus == null) throw new ArgumentException("Cannot find corresponding next status for " + quote.Status.Name);
            UpdateStatusAndHistory(quote, user, description, (int)nextstatus);
            try
            {
                await _quoteEmailService.SendPrintEmailToWp(quote, model.Url);
            }
            catch (Exception e) { }
            Save();
            return quote;
        }

        public async Task<Quote> Print(QuotePostModel model, string user)
        {
            if(model.Date==null) throw new ArgumentException("Complete Date cannot be empty");
            var description = "Quote is printed and send to BD/QP on " + model.Date.Value.ToString("dd/MM/yyyy");
            var quote = GetByKey(model.QuoteId);
            if (quote == null) throw new ArgumentException("Invalid Quote Id " + model.QuoteId);
            var nextstatus = NextStatus(quote, true);
            if (nextstatus == null) throw new ArgumentException("Cannot find corresponding next status for " + quote.Status.Name);
            quote.PrintDate = model.Date;
            UpdateStatusAndHistory(quote, user, description, (int)nextstatus);
            try
            {
                await _quoteEmailService.SendPresentClientAlertToQp(quote, model.Url);
            }
            catch (Exception e) { }
            Save();
            return quote;
        }
      
        public Quote CancelQuote(int quoteId, string user)
        {
            const string description = "Quote cancelled";
            const int nextStatusId = (int)QuoteStatusTypes.Cancel;
            var quote = GetByKey(quoteId);
            if (quote == null) throw new ArgumentException("Invalid Quote Id " + quoteId);
            quote.LastUpdateDate = DateTime.Now;
           
            UnitOfWork.GetRepository<QuoteHistory>().Add(new QuoteHistory
            {
                Description = description,
                Time = DateTime.Now,
                User = user,
                FromStatusId = quote.StatusId,
                ToStatusId = nextStatusId,
                QuoteId = quote.Id
            });
            quote.StatusId = nextStatusId;
            //Change corresponding lead status to Cancelled
            if (quote.Lead != null) quote.Lead.LeadStatusId = (int)LeadStatusTypes.Cancelled;
            Save();
            return quote;
        }

        public Quote Contact(QuotePostModel model)
        {
            var quote = GetByKey(model.QuoteId);
            if(quote==null) throw new ArgumentException("Invalid quote Id "+model.QuoteId);
            quote.ContactCheckOverDue = false;
            quote.LastContactCheckDate = DateTime.Now;
            quote.LastContactDate = model.Date;
            quote.LastUpdateDate = DateTime.Now;
            Save();
            return quote;
        }

        public Quote NotSendEmail(string user, int quoteId, List<QuestionResultModel> questionResults)
        {
            var quote = GetByKey(quoteId);
            if (quote == null) throw new ArgumentException("Invalid quote Id " + quoteId);
            UpdateQuestionResult(questionResults, quoteId, user, QuoteQuestionType.NoEmail);
            quote.LastClientEmailCheckDate = DateTime.Now;
            quote.LastUpdateDate = DateTime.Now;
            quote.ClientEmailSendDate = quote.ClientEmailSendDate == null
                ? quote.LastClientEmailCheckDate.Value.AddMonths(1)
                : quote.ClientEmailSendDate.Value.AddMonths(1);
            quote.ClientEmailSendReminderDisabled = false;
            Save();
            return quote;
        }

        public Quote NotContact(string user, int quoteId, List<QuestionResultModel> questionResults)
        {
            var quote = GetByKey(quoteId);
            if(quote==null) throw new ArgumentException("Invalid quote Id "+quoteId);
            UpdateQuestionResult(questionResults, quoteId, user, QuoteQuestionType.NotCalled);
            quote.ContactCheckOverDue = false;
            quote.LastContactCheckDate=DateTime.Now;
            quote.LastUpdateDate = DateTime.Now;
            Save();
            return quote;
        }

        public IList<QuoteQuestion> GetQuestionsByType(int questionType)
        {
            if (!Enum.IsDefined(typeof (QuoteQuestionType), questionType)) throw new ArgumentException("Invalid question Type "+questionType);
            var type = (QuoteQuestionType) questionType;
            return UnitOfWork.GetRepository<QuoteQuestion>().Get(l => l.Type == type);
        }

        public Quote Finalize(string user,int quoteId, List<QuestionResultModel> questionResults)
        {
            const string description = "Quote became a current quote";
            var quote = GetByKey(quoteId);
            if (quote == null) throw new ArgumentException("invalid quote Id " + quoteId);
            var nextstatus = NextStatus(quote, true);
            if (nextstatus == null) throw new ArgumentException("Cannot find corresponding next status for " + quote.Status.Name);
            UpdateQuestionResult(questionResults,quoteId,user,QuoteQuestionType.ToCurrent);
            UpdateStatusAndHistory(quote, user, description, (int)nextstatus);
            Save();
            return quote;
        }


        #region private functions
        private void UpdateQuestionResult(List<QuestionResultModel> questionResults, int quoteId, string user, QuoteQuestionType type)
        {
            //Check answer all the questions or not
            var questionResultIds = questionResults.Select(l => l.QuestionId).OrderBy(l => l).ToArray();
            var questionIds = UnitOfWork.GetRepository<QuoteQuestion>().Filter(l => l.Type == type, l => l.Id, null).OrderBy(l => l).ToArray();
            if (!questionResultIds.SequenceEqual(questionIds)) throw new ArgumentException("Invalid Question Counts. Please answer all the questions");
            var questionResultRepo = UnitOfWork.GetRepository<QuoteQuestionResult>();
            var time = DateTime.Now;
            //Add new result
            foreach (var result in questionResults)
            {
                var temp = new QuoteQuestionResult
                {
                    AnswerId = result.AnswerId,
                    QuestionId = result.QuestionId,
                    QuoteId = quoteId,
                    Additional = result.Additional,
                    Time = time,
                    User = user
                };
                questionResultRepo.Add(temp);
            }
        }

        private static string GetFileName(Quote quote, string extension,string suffix=null)
        {
            if (suffix != null) return quote.Company + @suffix + extension;
               return quote.Company + extension;
        }


        private QuoteIssue ResolveIssue(string user, ResolveModel model)
        {
            var issue = UnitOfWork.GetRepository<QuoteIssue>().Get(model.IssueId);
            if (issue == null) throw new ArgumentException("invalid Issue Id " + model.IssueId);
            var nextstatus = NextStatus(issue.Quote, true, issue.Id);
            if (nextstatus == null) throw new ArgumentException("Cannot find corresponding next status for " +issue.NextStatus.Name);
            string description;
            if (nextstatus.Value ==  QuoteStatusTypes.Print)
            {
                description = issue.UploadPricePageOnly ? "Reprint Price Page Only" : "Reprint full quote";
            }
            else
            {
                description = model.Comment;
            }
            UpdateStatusAndHistory(issue.Quote, user, description, (int)nextstatus);
            issue.SolvedBy = user;
            issue.SolvedDate = DateTime.Now;
            issue.SolverComments = model.Comment;
            issue.Resolved = true;
            return issue;
        }


        private void UploadQuotePricePage(FileInfo tempFile, string originalFileName, Quote quote)
        {
            var path = AppDomain.CurrentDomain.BaseDirectory + _settings.QuoteUploadPath + @"/" + quote.Id;
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            try
            {
                var fileName = GetFileName(quote, Path.GetExtension(originalFileName), "(pricePage)").FilterInvalidFileName();
                tempFile.CopyTo(Path.Combine(path, fileName), true);
                quote.PricePageName = fileName;
            }
            finally
            {
                tempFile.Delete();
            }
        }


        private void AddQuoteIssue(QuoteIssue issue,Quote quote)
        {
            if (quote.QuoteIssues.Any(l => l.Resolved == false))
            {
                throw new ArgumentException("This quote has existing outstanding issue. You must resolve it first");
            }
            UnitOfWork.GetRepository<QuoteIssue>().Add(issue);
        }

        private void UploadQuoteDocumment(FileInfo tempFile, string originalFileName, Quote quote)
        {
            var path = AppDomain.CurrentDomain.BaseDirectory + _settings.QuoteUploadPath + @"/" + quote.Id;
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            try
            {
                var fileName = GetFileName(quote, Path.GetExtension(originalFileName)).FilterInvalidFileName();
                tempFile.CopyTo(Path.Combine(path, fileName), true);
                quote.FileName = fileName;
                quote.LastUploadDate = DateTime.Now;
            }
            finally
            {
                tempFile.Delete();
            }
        }

        private QuoteStatusTypes? NextIssueStatus(Quote quote)
        {
            QuoteStatusTypes? nextStatus = null;
            var currentStatus = quote.StatusId;
            switch (currentStatus)
            {
                case (int)QuoteStatusTypes.WPReview:
                    nextStatus = (QuoteStatusTypes)currentStatus;
                    break;
                case (int)QuoteStatusTypes.QPReview:
                    nextStatus = (QuoteStatusTypes)currentStatus;
                    break;
                case (int)QuoteStatusTypes.BDReview:
                    nextStatus = (QuoteStatusTypes)currentStatus;
                    break;
                case (int)QuoteStatusTypes.FinalReview:
                    nextStatus = (QuoteStatusTypes)currentStatus;
                    break;
                case (int)QuoteStatusTypes.PresentToClient:
                    nextStatus = QuoteStatusTypes.Print;
                    break;
                case (int)QuoteStatusTypes.Current:
                    nextStatus = QuoteStatusTypes.Print;
                    break;
            }
            return nextStatus;
        }
        
        #endregion private functions
    }
}
