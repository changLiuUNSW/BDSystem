using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using DataAccess.Common.SearchModels;
using DataAccess.EntityFramework.Expressions;
using DataAccess.EntityFramework.Infrastructure;
using DataAccess.EntityFramework.Models.BD.Site;
using Site = DataAccess.EntityFramework.Models.BD.Site.Site;

namespace DataAccess.EntityFramework.Repositories
{
    public interface ISiteRepository : IRepository<Site>
    {
        Task<SearchResult<AdminSearch>> Search(Search search, string bizType);
        List<Site> GetSiteKey(string key,int? take);
        IList<SiteGroup> SearchGroupManager(IList<SearchField> fields);
    }

    internal class SiteRepository : Repository<Site>, ISiteRepository
    {
        internal SiteRepository(IDbContext dbContext) : base(dbContext){}

        public IList<SiteGroup> SearchGroupManager(IList<SearchField> fields)
        {
            return DataContext.DbSet<SiteGroup>().SearchByFields(fields, true).ToList();
        }

        public async Task<SearchResult<AdminSearch>> Search(Search search, string bizType)
        {
            //Project & Filter    
            IQueryable<AdminSearch> query = ProjectFilters(bizType, DbSet.AsQueryable())
                .SearchByFields(search.SearchFields);
            //order
            query = !string.IsNullOrEmpty(search.SortColumn)
                ? query.OrderByField(search.SortColumn, search.Order != "desc")
                : query.OrderByField("key", true);

            //TODO: may need to evaluate whether its worthwhile to do in-memory count
            return new SearchResult<AdminSearch>
            {
                Total = query.Count(),
                List = await query.Skip((search.CurrentPage - 1)*search.PageSize)
                    .Take(search.PageSize).ToListAsync()
            };
        }

        public List<Site> GetSiteKey(string key, int? take)
        {
            EnableProxyCreation(false);
            var query = DbSet.AsQueryable();
            if (!string.IsNullOrEmpty(key)) query=query.Where(l => l.Key.StartsWith(key));
            if (take != null) query=query.Take((int)take);
            var result = query.OrderBy(l=>l.Key).ToList();
            EnableProxyCreation(false);
            return result;
        }


        private IQueryable<AdminSearch> ProjectFilters(string bizType, IQueryable<Site> query)
        {
            query=query.Where(l => l.Contacts.Count(j => j.BusinessType.Type.ToLower() == bizType.ToLower()) > 0);
            IQueryable<AdminSearch> result = query.Select(l => new AdminSearch
            {
                //Site
                BusinessType = bizType,
                SiteId =  l.Id,
                Key = l.Key,
                SalesRepList = l.Contacts.Where(x => x.BusinessType.Type == bizType&&x.Code != null).Select(j => j.Code).Distinct(),
                Company = l.Name,
                Unit = l.Unit,
                Number =  l.Number,
                Street = l.Street,
                Suburb = l.Suburb,
                State = l.State,
                PostCode = l.Postcode,
                Phone = l.Phone 
            });
            return result;
        }
    }
}