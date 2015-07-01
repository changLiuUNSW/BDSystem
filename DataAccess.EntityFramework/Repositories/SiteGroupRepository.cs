using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DataAccess.EntityFramework.Infrastructure;
using DataAccess.EntityFramework.Models.BD.Site;

namespace DataAccess.EntityFramework.Repositories
{
    public interface ISiteGroupRepository : IRepository<SiteGroup>
    {
        List<SiteGroup> Search(string type, string code);
        SiteGroup GetGroupById(int id);
    }

    internal class SiteGroupRepository : Repository<SiteGroup>, ISiteGroupRepository
    {
        internal SiteGroupRepository(IDbContext dbContext) : base(dbContext){}

        public List<SiteGroup> Search(string type, string code)
        {
            EnableProxyCreation(false);
            var query = DbSet.AsQueryable();
            if (!string.IsNullOrEmpty(type))query=query.Where(l => l.Type.ToLower() == type.ToLower());
            if (!string.IsNullOrEmpty(code)) query = query.Where(l => l.Code.ToLower() == code.ToLower());
            EnableProxyCreation(true);
            return query.ToList();
        }

        public SiteGroup GetGroupById(int id)
        {
            EnableProxyCreation(false);
            var siteGroup= DbSet.Where(l => l.Id == id).Include(l => l.Sites).SingleOrDefault();
            EnableProxyCreation(true);
            return siteGroup;
        }
    }
}
