using System;
using System.Linq;
using DataAccess.EntityFramework;
using model = DataAccess.EntityFramework.Models.BD.Site;

namespace DateAccess.Services.SiteService
{

    public class SiteGroupModel
    {
        public int SiteId;
        public int? OrignGroupId;
        public int? NewGroupId;
    }

    internal class SiteService : ISiteService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SiteService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public model.Site GetSite(int id)
        {
            return _unitOfWork.SiteRepository.Get(id);
        }

        public model.Site GetSiteByKey(string key)
        {
            return _unitOfWork.SiteRepository.SingleOrDefault(l => l.Key == key);
        }

        public int UpdateGroup(SiteGroupModel siteGroupModel)
        {
            var site = _unitOfWork.SiteRepository.Get(siteGroupModel.SiteId);
            if (siteGroupModel.OrignGroupId != null)
            {
                var orignGroup = site.Groups.SingleOrDefault(l => l.Id == siteGroupModel.OrignGroupId);
                if(orignGroup!=null)site.Groups.Remove(orignGroup);
            }
            if (siteGroupModel.NewGroupId != null)
            {
                var newGroup = _unitOfWork.GetRepository<model.SiteGroup>().Get(siteGroupModel.NewGroupId);
                if (site.Groups.Any(l => l.Type.ToLower() == newGroup.Type.ToLower())) 
                    throw new Exception("This site has already been assigned to another group");
                if(newGroup!=null) site.Groups.Add(newGroup);
            }
            return _unitOfWork.Save();
        }
    }

    public interface ISiteService
    {
        model.Site GetSite(int id);

        model.Site GetSiteByKey(string key);

        int UpdateGroup(SiteGroupModel siteGroupModel);
    }
}