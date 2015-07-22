using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DataAccess.Common.SearchModels;
using DataAccess.EntityFramework.Expressions;
using DataAccess.EntityFramework.Infrastructure;
using DataAccess.EntityFramework.Models.BD.Site;
using Site = DataAccess.EntityFramework.Models.BD.Site.Site;

namespace DataAccess.EntityFramework.Repositories
{
    public interface ISiteRepository : IRepository<Site>
    {
        List<Site> GetSiteKey(string key,int? take);
        List<Site> SearchSiteAddr(string keyword,int? take);
        Site CheckSiteExists(
            string company,
            string unit,
            string number,
            string street,
            string suburb,
            string postcode,
            string state);
    }

    internal class SiteRepository : Repository<Site>, ISiteRepository
    {
        internal SiteRepository(DbContext dbContext) : base(dbContext){}

        public List<Site> SearchSiteAddr(string keyword, int? take)
        {
            EnableProxyCreation(false);
            var query = DbSet.AsQueryable();
            if (!string.IsNullOrEmpty(keyword))
            {
                query = DbSet.Select(l => new
                {
                    site = l, 
                    address = l.Unit + " " + l.Number + " " + l.Street + " " + l.Suburb + " " + l.State + " " + l.Postcode
                })
                .Where(l => l.address.Contains(keyword)||l.site.Name.Contains(keyword))
                .Select(l=>l.site);
            }
            if (take != null) query=query.Take(take.Value);
    
            var result= query.ToList();
            EnableProxyCreation(true);
            return result;
        }

        public Site CheckSiteExists(string company, string unit, string number, string street, string suburb, string postcode,
            string state)
        {
            return DbSet.FirstOrDefault(l => l.Name.ToLower() == company.ToLower() &&
                                             l.Unit.ToLower() == unit.ToLower() &&
                                             l.Number.ToLower() == number.ToLower() &&
                                             l.Street.ToLower() == street.ToLower() &&
                                             l.Suburb.ToLower() == suburb.ToLower() &&
                                             l.State.ToLower() == state.ToLower() &&
                                             l.Postcode.ToLower() == postcode.ToLower());
        }

        public List<Site> GetSiteKey(string key, int? take)
        {
            EnableProxyCreation(false);
            var query = DbSet.AsQueryable();
            if (!string.IsNullOrEmpty(key)) query=query.Where(l => l.Key.StartsWith(key));
            if (take != null) query=query.Take((int)take);
            var result = query.OrderBy(l=>l.Key).ToList();
            EnableProxyCreation(true);
            return result;
        }
    }
}