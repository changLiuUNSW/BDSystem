using System.Collections.Generic;
using System.Linq;
using DataAccess.EntityFramework;
using DataAccess.EntityFramework.Models.BD.Site;

namespace DateAccess.Services.SiteService
{
    internal class SiteGroupService : RepositoryService<SiteGroup>,ISiteGroupService
    {
        public SiteGroupService(IUnitOfWork unitOfWork) : base(unitOfWork){}
        public List<SiteGroup> Search(string type, string code)
        {
            return UnitOfWork.SiteGroupRepository.Search(type, code);
        }

        public int RemoveSites(int id,int [] siteIds)
        {
            var group = UnitOfWork.SiteGroupRepository.Get(id);
            foreach (var siteId in siteIds)
            {
                var site = group.Sites.SingleOrDefault(l => l.Id == siteId);
                if (site != null) group.Sites.Remove(site);
            }
            return UnitOfWork.Save();
        }
    }

    public interface ISiteGroupService:IRepositoryService<SiteGroup>
    {
        List<SiteGroup> Search(string type, string code);
        int RemoveSites(int id,int [] siteIds);
    }
}