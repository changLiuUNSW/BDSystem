using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Common.SearchModels;
using DataAccess.EntityFramework.Expressions;
using DataAccess.EntityFramework.Infrastructure;
using DataAccess.EntityFramework.Models.BD.Lead;

namespace DataAccess.EntityFramework.Repositories
{
    public class LeadSearchModel
    {
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public string Order { get; set; }
        public string SortColumn { get; set; }
        public string Status { get; set; }
        public string LeadType { get; set; }
        public string Keyword { get; set; }
    }

    public interface ILeadRepository : IRepository<Lead>
    {
        Task<SearchResult<LeadSearch>> SearchLead(LeadSearchModel leadSearchModel);
    }



    internal class LeadRepository : Repository<Lead>, ILeadRepository
    {
        public LeadRepository(DbContext dbContext) : base(dbContext){}

        public async Task<SearchResult<LeadSearch>> SearchLead(LeadSearchModel leadSearchModel)
        {
            var query = Project(DbSet.AsQueryable());
            if (!string.IsNullOrEmpty(leadSearchModel.Status))
            {
                query = query.Where(l => l.Status.ToLower() == leadSearchModel.Status.ToLower());
            }
            else
            {
                query = query.Where(l => l.Hidden==false);
            }
            if (!string.IsNullOrEmpty(leadSearchModel.LeadType)) query = query.Where(l => l.LeadType.ToLower() == leadSearchModel.LeadType.ToLower());
            if (!string.IsNullOrEmpty(leadSearchModel.Keyword)) query = Filter(query, leadSearchModel.Keyword);
            query = !string.IsNullOrEmpty(leadSearchModel.SortColumn) ?
                query.OrderByField(leadSearchModel.SortColumn, leadSearchModel.Order != "desc") 
                 :query.OrderByField("LastUpdateDate", false);
            return new SearchResult<LeadSearch>
            {
                Total =await query.CountAsync(),
                List = await query.Skip((leadSearchModel.CurrentPage - 1) * leadSearchModel.PageSize)
                    .Take(leadSearchModel.PageSize).ToListAsync()
            };
        }

        private IQueryable<LeadSearch> Filter(IQueryable<LeadSearch> query,string keyword)
        {
            return query.
                Select(l => new
                {
                    entity = l,
                    address = l.SiteUnit + " " +
                              l.SiteNumber + " " +
                              l.SiteStreet + " " +
                              l.SiteSuburb + " " +
                              l.SiteState + " " +
                              l.SitePostcode
                }).Where(l => l.entity.Id.ToString().StartsWith(keyword) ||
                                          l.entity.LeadType.StartsWith(keyword) ||
                                          l.entity.Phone.Contains(keyword) ||
                                          l.entity.Status.StartsWith(keyword) ||
                                          l.entity.FirstName.StartsWith(keyword) ||
                                          l.entity.LastName.StartsWith(keyword) ||
                                          l.entity.QpInitial.StartsWith(keyword) ||
                                          l.address.Contains(keyword)||
                                          l.entity.QpName.StartsWith(keyword)
                ).Select(l=>l.entity);
        }


        private IQueryable<LeadSearch> Project(IQueryable<Lead> query)
        {
            var result = query.Select(l => new LeadSearch
            {
                Id = l.Id,
                LastUpdateDate = l.LastUpdateDate,
                LeadType = l.BusinessType.Type,
                Status = l.LeadStatus.Name,
                Hidden = l.LeadStatus.Hidden,
                Phone = l.Phone,
                SiteName = l.Contact.Site.Name,
                SiteUnit = l.Address.Unit,
                SiteNumber = l.Address.Number,
                SiteStreet = l.Address.Street,
                SiteSuburb = l.Address.Suburb,
                SiteState = l.State,
                SitePostcode = l.Postcode,
                FirstName = l.Contact.ContactPerson.Firstname,
                LastName = l.Contact.ContactPerson.Lastname,
                QpInitial = l.LeadPersonal.Initial,
                QpName = l.LeadPersonal.Name
            });
            return result;
        }
    }
}
