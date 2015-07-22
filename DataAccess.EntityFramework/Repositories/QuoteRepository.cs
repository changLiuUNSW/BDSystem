using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Common.SearchModels;
using DataAccess.EntityFramework.Expressions;
using DataAccess.EntityFramework.Infrastructure;
using DataAccess.EntityFramework.Models.Quote;
using DataAccess.EntityFramework.TypeLibrary;

namespace DataAccess.EntityFramework.Repositories
{
    public abstract class QuoteSearchBase
    {
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public string Order { get; set; }
        public string SortColumn { get; set; }
        public string Keyword { get; set; }
        public string QuoteType { get; set; }
    }

    public class QuoteProgressSearchModel : QuoteSearchBase
    {
        public string Status { get; set; }
    }

    public class QuoteCurrentSearchModel : QuoteSearchBase
    {
        public bool ContactCheckOverDue { get; set; }
        public bool DeadCheckOverDue { get; set; }
        public bool AjustCheckOverDue { get; set; }
    }


    public class OverdueModel
    {
        public string Name { get; set; }
        public int Count { get; set; }
        public string Field { get; set; }
    }


    public class QuoteResultModel
    {
        public DateTime Time { get; set; }
        public QuoteQuestionType Type { get; set; }
        public List<QuoteQuestionResult> Results { get; set; } 

    }
    public interface IQuoteRepository : IRepository<Quote>
    {
        Task<SearchResult<QuoteProgressModel>> SearchProgressQuote(QuoteProgressSearchModel model);
        Task<SearchResult<QuoteCurrentModel>> SearchCurrentQuote(QuoteCurrentSearchModel model);
        Task<List<OverdueModel>> GetOverDueList();
        List<QuoteResultModel> GetQuoteResultListByType(int quoteId,QuoteQuestionType? type);
    }

    internal class QuoteRepository : Repository<Quote>,IQuoteRepository
    {
        public QuoteRepository(DbContext dbContext) : base(dbContext) { }


        public async Task<SearchResult<QuoteProgressModel>> SearchProgressQuote(QuoteProgressSearchModel model)
        {
            var result = DbSet.AsQueryable();
            if (!string.IsNullOrEmpty(model.Keyword)) result = QuoteFilter(result, model.Keyword);
            var query = QuoteProgressProject(result);
            if (!string.IsNullOrEmpty(model.Status))
            {
                query = query.Where(l => l.Status.ToLower() == model.Status.ToLower());
            }
            else
            {
                query = query.Where(l => l.Hidden==false);
            }
            if (!string.IsNullOrEmpty(model.QuoteType)) query = query.Where(l => l.QuoteType.ToLower() == model.QuoteType.ToLower());
            query = !string.IsNullOrEmpty(model.SortColumn) ?
              query.OrderByField(model.SortColumn, model.Order != "desc")
               : query.OrderByField("LastUpdateDate", false);
            return new SearchResult<QuoteProgressModel>
            {
                Total = await query.CountAsync(),
                List = await query.Skip((model.CurrentPage - 1) * model.PageSize)
                    .Take(model.PageSize).ToListAsync()
            };
        }

        public async Task<SearchResult<QuoteCurrentModel>> SearchCurrentQuote(QuoteCurrentSearchModel model)
        {
            var currentQuotes = DbSet.Where(l => l.StatusId == (int) QuoteStatusTypes.Current);
            var currentQuotesInIssues = DbSet.Where(l => l.StatusId == (int) QuoteStatusTypes.WPIssues && l.WasCurrent);
            var result = currentQuotes.Union(currentQuotesInIssues);
            if (!string.IsNullOrEmpty(model.Keyword))
            {
                result = QuoteFilter(result, model.Keyword);
            }
            var query = QuoteCurrentProject(result);
            if (model.AjustCheckOverDue) query = query.Where(l => l.AjustCheckOverDue);
            if (model.ContactCheckOverDue) query = query.Where(l => l.ContactCheckOverDue);
            if (model.DeadCheckOverDue) query = query.Where(l => l.DeadCheckOverDue);
            if (!string.IsNullOrEmpty(model.QuoteType)) query = query.Where(l => l.QuoteType.ToLower() == model.QuoteType.ToLower());
            query = !string.IsNullOrEmpty(model.SortColumn) ?
              query.OrderByField(model.SortColumn, model.Order != "desc")
               : query.OrderByField("LastContactDate", false);
            return new SearchResult<QuoteCurrentModel>
            {
                Total = await query.CountAsync(),
                List = await query.Skip((model.CurrentPage - 1)*model.PageSize)
                    .Take(model.PageSize).ToListAsync()
            };
        }

        public async Task<List<OverdueModel>> GetOverDueList()
        {
            var currentQuotes = DbSet.Where(l => l.StatusId == (int)QuoteStatusTypes.Current);
            var currentQuotesInIssues = DbSet.Where(l => l.StatusId == (int)QuoteStatusTypes.WPIssues && l.WasCurrent);
            var query = QuoteCurrentProject(currentQuotes.Union(currentQuotesInIssues));
            var contactCheckOverDue = new OverdueModel
            {
                Count = await query.Where(l => l.ContactCheckOverDue).CountAsync(),
                Name = "Contact",
                Field = "ContactCheckOverDue"
            };
            var ajustCheckOverDue = new OverdueModel
            {
                Count = await query.Where(l => l.AjustCheckOverDue).CountAsync(),
                Name = "Ajust",
                Field = "AjustCheckOverDue"
            };
            var deadCheckOverDue = new OverdueModel
            {
                Count = await query.Where(l => l.DeadCheckOverDue).CountAsync(),
                Name = "Dead",
                Field = "DeadCheckOverDue"
            };
            return new List<OverdueModel> { contactCheckOverDue, ajustCheckOverDue, deadCheckOverDue };
        }

        public List<QuoteResultModel> GetQuoteResultListByType(int quoteId,QuoteQuestionType? type)
        {
            var result =  DataContext.Set<QuoteQuestionResult>().Where(l => l.QuoteId == quoteId);
            if (type != null) result=result.Where(l => l.Question.Type == type);
            return  result
                .GroupBy(
                l => new {l.Time,l.Question.Type},
                    (k, g) => new QuoteResultModel
                    {
                        Time = k.Time,
                        Type = k.Type,
                        Results = g.ToList()
                    }
                ).OrderByDescending(l=>l.Time).ToList();
        }


        private IQueryable<Quote> QuoteFilter(IQueryable<Quote> query, string keyword)
        {
            return query.Select(l => new
            {
                entity = l,
                address = l.Address.Unit + " " +
                          l.Address.Number + " " +
                          l.Address.Street + " " +
                          l.Address.Suburb + " " +
                          l.State + " " +
                          l.Postcode
            }).Where(l => l.entity.Id.ToString().StartsWith(keyword) ||
                                    l.entity.BusinessType.Type.StartsWith(keyword) ||
                                    l.entity.Status.Name.StartsWith(keyword) ||
                                    l.entity.Phone.Contains(keyword) ||
                                    l.entity.Company.Contains(keyword) ||
                                    l.address.Contains(keyword)||
                                    l.entity.LeadPersonal.Initial.StartsWith(keyword) ||
                                    l.entity.LeadPersonal.Name.StartsWith(keyword)    
                ).Select(l=>l.entity);
        }



       

        private IQueryable<QuoteCurrentModel> QuoteCurrentProject(IQueryable<Quote> query)
        {
            var result = query.Select(l => new QuoteCurrentModel
            {
                Id = l.Id,
                LastUpdateDate = l.LastUpdateDate,
                QuoteType = l.BusinessType.Type,
                StatusId = l.Status.Id,
                Status = l.Status.Name,
                Phone = l.Phone,
                Company = l.Company,
                Unit = l.Address.Unit,
                Number = l.Address.Number,
                Street = l.Address.Street,
                Suburb = l.Address.Suburb,
                State = l.State,
                Postcode = l.Postcode,
                QpInitial = l.LeadPersonal.Initial,
                QpName = l.LeadPersonal.Name,
                TotalPA = l.QuoteCost.Sum(j=>j.PricePa),
                TotalPW = l.QuoteCost.Sum(j=>j.ReturnPw),
                LastestPA = l.LastestPA,
                SuccessRate = l.SuccessRate,
                ContactCheckOverDue = l.ContactCheckOverDue,
                DeadCheckOverDue = l.DeadCheckOverDue,
                AjustCheckOverDue = l.AjustCheckOverDue,
                LastContactDate=l.LastContactDate
            });
            return result;
        }


        private IQueryable<QuoteProgressModel> QuoteProgressProject(IQueryable<Quote> query)
        {
            var result = query.Select(l => new QuoteProgressModel
            {
                Id = l.Id,
                LastUpdateDate = l.LastUpdateDate,
                QuoteType = l.BusinessType.Type,
                Status = l.Status.Name,
                Hidden=l.Status.Hidden,
                Phone = l.Phone,
                Company = l.Company,
                Unit = l.Address.Unit,
                Number = l.Address.Number,
                Street = l.Address.Street,
                Suburb = l.Address.Suburb,
                State = l.State,
                Postcode = l.Postcode,
                Firstname = l.Firstname,
                Lastname = l.Lastname,
                QpInitial = l.LeadPersonal.Initial,
                QpName = l.LeadPersonal.Name,
                TotalPA = l.QuoteCost.Sum(j=>j.PricePa),
                TotalPW=l.QuoteCost.Sum(j=>j.ReturnPw),
                LastestPA = l.LastestPA
            });
            return result;
        }
    }
}