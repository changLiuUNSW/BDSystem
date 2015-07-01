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

    public interface IQuoteRepository : IRepository<Quote>
    {
        Task<SearchResult<QuoteProgressModel>> SearchProgressQuote(QuoteProgressSearchModel model);
        Task<SearchResult<QuoteCurrentModel>> SearchCurrentQuote(QuoteCurrentSearchModel model);
        Task<List<OverdueModel>> GetOverDueList();
    }

    internal class QuoteRepository : Repository<Quote>,IQuoteRepository
    {
        public QuoteRepository(IDbContext dbContext) : base(dbContext) { }


        public async Task<SearchResult<QuoteProgressModel>> SearchProgressQuote(QuoteProgressSearchModel model)
        {
            var query = QuoteProgressProject(DbSet.AsQueryable());
            if (!string.IsNullOrEmpty(model.Status))
            {
                query = query.Where(l => l.Status.ToLower() == model.Status.ToLower());
            }
            else
            {
                query = query.Where(l => l.Hidden==false);
            }
            if (!string.IsNullOrEmpty(model.QuoteType)) query = query.Where(l => l.QuoteType.ToLower() == model.QuoteType.ToLower());
            if (!string.IsNullOrEmpty(model.Keyword)) query = QuoteFilter(query, model.Keyword) as IQueryable<QuoteProgressModel>;
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
            var query = QuoteCurrentPorject(currentQuotes.Union(currentQuotesInIssues));
            if (model.AjustCheckOverDue) query = query.Where(l => l.AjustCheckOverDue);
            if (model.ContactCheckOverDue) query = query.Where(l => l.ContactCheckOverDue);
            if (model.DeadCheckOverDue) query = query.Where(l => l.DeadCheckOverDue);
            if (!string.IsNullOrEmpty(model.QuoteType)) query = query.Where(l => l.QuoteType.ToLower() == model.QuoteType.ToLower());
            if (!string.IsNullOrEmpty(model.Keyword)) query = QuoteFilter(query, model.Keyword) as IQueryable<QuoteCurrentModel>;
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
            var query = QuoteCurrentPorject(currentQuotes.Union(currentQuotesInIssues));
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

        private IQueryable<QuoteModelBase> QuoteFilter(IQueryable<QuoteModelBase> query, string keyword)
        {
            return query.Where(l => l.Id.ToString().StartsWith(keyword) ||
                                    l.QuoteType.StartsWith(keyword) ||
                                    l.Status.StartsWith(keyword) ||
                                    l.Phone.Contains(keyword) ||
                                    l.Company.Contains(keyword) ||
                                    l.Unit.StartsWith(keyword) ||
                                    l.Number.StartsWith(keyword) ||
                                    l.Street.Contains(keyword) ||
                                    l.Suburb.StartsWith(keyword) ||
                                    l.State.StartsWith(keyword) ||
                                    l.Postcode.Contains(keyword) ||
                                    l.QpInitial.StartsWith(keyword) ||
                                    l.QpName.StartsWith(keyword)||
                                    l.TotalPW.ToString().StartsWith(keyword)
                );
        }



       

        private IQueryable<QuoteCurrentModel> QuoteCurrentPorject(IQueryable<Quote> query)
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