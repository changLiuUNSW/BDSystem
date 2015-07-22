using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using DataAccess.EntityFramework.DbContexts;
using DataAccess.EntityFramework.Models.BD;
using DataAccess.EntityFramework.Models.Quote;
using DataAccess.EntityFramework.TypeLibrary;

namespace DataAccess.EntityFramework.Migrations
{
    internal static class QuoteInitializer
    {
        public static void Start(SiteResourceEntities context)
        {
            StatusInit(context);
//            TestInit(context);
            QuestionInit(context);
        }

        private static void TestInit(SiteResourceEntities context)
        {
            var address = new Address
            {
                Suburb = "wellington",
                Street = "Shoreline",
                Number = "12",
                Unit = "13"
            };
            var quote = new Quote
            {
                CreatedDate = DateTime.Today,
                LastUpdateDate = DateTime.Today,
                BusinessTypeId = 1,
                Firstname = "chang",
                Lastname = "liu",
                Position = "manager",
                Title = "Mr",
                Company = "NZ test company",
                LeadPersonalId = 1,
                StatusId = 1,
                Phone = "231414124",
                Postcode = "0112",
                State = "NZ",
                Address = address
            };
            context.Quotes.AddOrUpdate(l=>l.BusinessTypeId,quote);
        }


        private static void QuestionInit(SiteResourceEntities context)
        {
            //ToCurrent questions
            var answerList1 = new List<QuoteAnswer>
            {
                new QuoteAnswer
                {
                   Name = "Face to face presentation",
                   Type = QuoteAnswerType.None
                },
                new QuoteAnswer
                {
                    Name="Hand delivered",
                    Type = QuoteAnswerType.None
                },
                new QuoteAnswer
                {
                    Name="Emailed quote",
                    Type = QuoteAnswerType.None
                },
                new QuoteAnswer
                {
                    Name = "Posted quote",
                    Type = QuoteAnswerType.None
                }
            };

            var question1 = new QuoteQuestion
            {
                Name = "How do you present the quote to the client ?",
                Type = QuoteQuestionType.ToCurrent,
                QuoteAnswers = answerList1
            };

            var answerList2 = new List<QuoteAnswer>
            {
                new QuoteAnswer
                {
                    Name = "High",
                    Type = QuoteAnswerType.None,
                },
                new QuoteAnswer
                {
                    Name = "Low",
                    Type = QuoteAnswerType.None,
                },
                new QuoteAnswer
                {
                    Name = "Same",
                    Type = QuoteAnswerType.None,
                },
                new QuoteAnswer
                {
                    Name = "Do not know",
                    Type = QuoteAnswerType.None,
                },
            };
            var question2 = new QuoteQuestion
            {
                Name = "How is our price in relation to what you are paying now ?",
                Type = QuoteQuestionType.ToCurrent,
                QuoteAnswers = answerList2
            };


            var answerList3 = new List<QuoteAnswer>
            {
                new QuoteAnswer
                {
                    Name = "Yes",
                    Type = QuoteAnswerType.None,
                },
                new QuoteAnswer
                {
                    Name = "No",
                    Type = QuoteAnswerType.None,
                }
            };
            var question3 = new QuoteQuestion
            {
                Name = "Do we resubmit a price ?",
                Type = QuoteQuestionType.ToCurrent,
                QuoteAnswers = answerList3
            };

            context.QuoteQuestions.AddOrUpdate(l => new { l.Name, l.Type }, question1, question2, question3);
            //Not called
            var answerList4 = new List<QuoteAnswer>
            {
                new QuoteAnswer
                {
                    Name = "Client does not want to be called",
                    Type = QuoteAnswerType.None
                },
                new QuoteAnswer
                {
                    Name = "I have tried and cannot contact",
                    Type = QuoteAnswerType.None
                },
                new QuoteAnswer
                {
                    Name = "Client has asked to be called on a specific date",
                    Type = QuoteAnswerType.Date
                }
            };
            var question4 = new QuoteQuestion
            {
                Name = "Reason why not called",
                Type = QuoteQuestionType.NotCalled,
                QuoteAnswers = answerList4
            };

            context.QuoteQuestions.AddOrUpdate(l => new { l.Name, l.Type }, question4);

            //No Dead
            var answerList5 = new List<QuoteAnswer>
            {
                new QuoteAnswer
                {
                    Name = "Official tender",
                    Type = QuoteAnswerType.None
                },
                new QuoteAnswer
                {
                    Name = "Client still considering",
                    Type = QuoteAnswerType.None
                },
                new QuoteAnswer
                {
                    Name = "No feedback but not dead",
                    Type = QuoteAnswerType.None
                },
                new QuoteAnswer
                {
                    Name = "Other",
                    Type = QuoteAnswerType.Text
                }
            };
            var question5 = new QuoteQuestion
            {
                Name = "Reason why not dead",
                Type = QuoteQuestionType.NoDead,
                QuoteAnswers = answerList5
            };

            context.QuoteQuestions.AddOrUpdate(l => new { l.Name, l.Type }, question5);

            //Dead
            var answerList6 = new List<QuoteAnswer>
            {
                new QuoteAnswer
                {
                    Name = "Yes",
                    Type = QuoteAnswerType.None
                },
                new QuoteAnswer
                {
                    Name = "No",
                    Type = QuoteAnswerType.None
                }
            };

            var question6 = new QuoteQuestion
            {
                Name = "Did the client stay with their current contractors ?",
                Type = QuoteQuestionType.Dead,
                QuoteAnswers = answerList6
            };
            var answerList7 = new List<QuoteAnswer>
            {
                new QuoteAnswer
                {
                    Name = "Yes",
                    Type = QuoteAnswerType.None
                },
                new QuoteAnswer
                {
                    Name = "No",
                    Type = QuoteAnswerType.None
                }
            };
            var question7 = new QuoteQuestion
            {
                Name = "Was our price higher than the client current paying ?",
                Type = QuoteQuestionType.Dead,
                QuoteAnswers = answerList7
            };
            var answerList8 = new List<QuoteAnswer>
            {
                new QuoteAnswer
                {
                    Name = "Yes",
                    Type = QuoteAnswerType.None
                },
                new QuoteAnswer
                {
                    Name = "No",
                    Type = QuoteAnswerType.None
                }
            };
            var question8 = new QuoteQuestion
            {
                Name = "Was it more than 10% ?",
                Type = QuoteQuestionType.Dead,
                QuoteAnswers = answerList8
            };
            var answerList9 = new List<QuoteAnswer>
            {
                new QuoteAnswer
                {
                    Name = "Yes",
                    Type = QuoteAnswerType.None
                },
                new QuoteAnswer
                {
                    Name = "No",
                    Type = QuoteAnswerType.None
                }
            };
            var question9 = new QuoteQuestion
            {
                Name = "Were the client satisfied with our proposal/response ?",
                Type = QuoteQuestionType.Dead,
                QuoteAnswers = answerList9
            };

            var question10 = new QuoteQuestion
            {
                Name = "Client Comment",
                Type = QuoteQuestionType.Dead,
                TextOnly = true
            };
            context.QuoteQuestions.AddOrUpdate(l => new { l.Name, l.Type }, question6, question7, question8, question9, question10);
           //Not Adjustment
           var answerList11 = new List<QuoteAnswer>
            {
                new QuoteAnswer
                {
                    Name = "Official tender",
                    Type = QuoteAnswerType.None
                },
                new QuoteAnswer
                {
                    Name = "Do not feel comfortable to ask",
                    Type = QuoteAnswerType.None
                },
                new QuoteAnswer
                {
                    Name = "Previous negative prices check response",
                    Type = QuoteAnswerType.None
                },
                new QuoteAnswer
                {
                    Name = "Other",
                    Type = QuoteAnswerType.Text
                }
            };
            var question11 = new QuoteQuestion
            {
                Name = "Reason why not adjust the quote ?",
                Type = QuoteQuestionType.NoAdjust,
                QuoteAnswers = answerList11
            };
            context.QuoteQuestions.AddOrUpdate(l => new { l.Name, l.Type }, question11);

            //Client Email
            var answerList12 = new List<QuoteAnswer>
            {
                new QuoteAnswer
                {
                    Name = "Currently negotiating",
                    Type = QuoteAnswerType.None
                },
                new QuoteAnswer
                {
                    Name = "Spoke recently",
                    Type = QuoteAnswerType.None
                },
                new QuoteAnswer
                {
                    Name = "Inappropriate for tihs quote",
                    Type = QuoteAnswerType.Text
                }
            };
            var question12 = new QuoteQuestion
            {
                Name = "Reason why not send Email ?",
                Type = QuoteQuestionType.NoEmail,
                QuoteAnswers = answerList12
            };
            context.QuoteQuestions.AddOrUpdate(l=>new {l.Name,l.Type},question12);
        }


        private static void StatusInit(SiteResourceEntities context)
        {
            context.QuoteStatus.AddOrUpdate(l => l.Name,
                new QuoteStatus
                {
                    Name = QuoteStatusTypes.New.ToString(),
                    Description = "Please create cost estimations",
                    Order = 1,
                },
                new QuoteStatus
                {
                    Name = QuoteStatusTypes.Estimation.ToString(),
                    Description = "Please finalize all the cost estimations and send to WP",
                    Order = 2,
                },
                new QuoteStatus
                {
                    Name = QuoteStatusTypes.WPReview.ToString(),
                    Description =
                        "Please review all the cost estimations. If there is no poblems, please merge data to quote and upload",
                    Order = 3,
                },
                new QuoteStatus
                {
                    Name = QuoteStatusTypes.QPReview.ToString(),
                    Description = "Please review the quote document",
                    Order = 4,
                },
                new QuoteStatus
                {
                    Name = QuoteStatusTypes.BDReview.ToString(),
                    Description =
                        "Please review the quote document",
                    Order = 5,
                },
                  new QuoteStatus
                  {
                      Name = QuoteStatusTypes.PreFinalReview.ToString(),
                      Description = "Please review the quote document before final review",
                      Order = 6,
                  },
                new QuoteStatus
                {
                    Name = QuoteStatusTypes.FinalReview.ToString(),
                    Description = "Please final review the quote document",
                    Order = 7,
                },
                new QuoteStatus
                {
                    Name = QuoteStatusTypes.Print.ToString(),
                    Description = "Please print the quote and send to QP",
                    Order = 8,
                },
                new QuoteStatus
                {
                    Name = QuoteStatusTypes.PresentToClient.ToString(),
                    Description = "Please present the quote to the client and get feedback from the client",
                    Order = 9,
                },
                new QuoteStatus
                {
                    Name = QuoteStatusTypes.WPIssues.ToString(),
                    Description = "Please resolve the outstanding issue",
                    Order = 10,
                },
                new QuoteStatus
                {
                    Name = QuoteStatusTypes.QPIssues.ToString(),
                    Description = "Please resolve the outstanding issue",
                    Order = 11,
                },
                new QuoteStatus
                {
                    Name = QuoteStatusTypes.Current.ToString(),
                    Description = null,
                    Order = 12,
                    Hidden = true
                },
                new QuoteStatus
                {
                    Name = QuoteStatusTypes.Cancel.ToString(),
                    Description = "This quote has been cancelled",
                    Order = 13,
                    Hidden = true
                },
                new QuoteStatus
                {
                    Name = QuoteStatusTypes.Dead.ToString(),
                    Description = "This quote is dead",
                    Order = 14,
                    Hidden = true
                }
                );
        }
    }
}