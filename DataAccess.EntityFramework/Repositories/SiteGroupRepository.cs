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
    }

    internal class SiteGroupRepository : Repository<SiteGroup>, ISiteGroupRepository
    {
        internal SiteGroupRepository(DbContext dbContext) : base(dbContext){}

        public List<SiteGroup> Search(string type, string code)
        {
            var query = DbSet.AsQueryable();
            if (!string.IsNullOrEmpty(type))query=query.Where(l => l.Type.ToLower() == type.ToLower());
            if (!string.IsNullOrEmpty(code)) query = query.Where(l => l.Code.ToLower() == code.ToLower());
            return query.Include(x => x.Sites).Include(x => x.ExternalManagers).ToList();
        }
    }
}
