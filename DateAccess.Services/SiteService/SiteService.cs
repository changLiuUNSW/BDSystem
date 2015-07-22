using System;
using System.Linq;
using DataAccess.EntityFramework;
using DataAccess.EntityFramework.Models.BD.Contact;
using Newtonsoft.Json.Linq;
using model = DataAccess.EntityFramework.Models.BD.Site;

namespace DateAccess.Services.SiteService
{

    public class SiteGroupModel
    {
        public int SiteId;
        public int? OrignGroupId;
        public int? NewGroupId;
    }

    internal class SiteService : RepositoryService<model.Site>, ISiteService
    {

        public SiteService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public model.Site GetSite(int id)
        {
            return UnitOfWork.SiteRepository.Get(id);
        }

        public model.Site GetSiteByKey(string key)
        {
            return UnitOfWork.SiteRepository.SingleOrDefault(l => l.Key == key);
        }

        public int UpdateGroup(SiteGroupModel siteGroupModel)
        {
            var site = UnitOfWork.SiteRepository.Get(siteGroupModel.SiteId);
            if (siteGroupModel.OrignGroupId != null)
            {
                var orignGroup = site.Groups.SingleOrDefault(l => l.Id == siteGroupModel.OrignGroupId);
                if(orignGroup!=null)site.Groups.Remove(orignGroup);
            }
            if (siteGroupModel.NewGroupId != null)
            {
                var newGroup = UnitOfWork.GetRepository<model.SiteGroup>().Get(siteGroupModel.NewGroupId);
                if (site.Groups.Any(l => l.Type.ToLower() == newGroup.Type.ToLower())) 
                    throw new Exception("This site has already been assigned to another group");
                if(newGroup!=null) site.Groups.Add(newGroup);
            }
            return UnitOfWork.Save();
        }

        public void UpdateSiteFromJson(JObject json)
        {
            if (json == null)
                throw new ArgumentNullException("json");

            var siteId = json["Id"];
            if (siteId == null)
                throw new Exception("Invalid site id");

            UpdateFromJson<model.Site>("Id", json);

            var contacts = json["Contacts"];
            var cleaningContract = json["CleaningContract"];
            var securityContract = json["SecurityContract"];

            if (contacts != null)
            {
                foreach (var child in json["Contacts"].Children<JObject>())
                {
                    UpdateFromJson<Contact>("Id", child);
                }
            }
            if (cleaningContract != null)
            {
                UpdateFromJson<model.CleaningContract>("SiteId", (JObject)cleaningContract);
            }
            if (securityContract != null)
            {
                UpdateFromJson<model.SecurityContract>("SiteId", (JObject)securityContract);
            }

            UnitOfWork.Save();
        }
    }

    public interface ISiteService : IRepositoryService<model.Site>
    {
        model.Site GetSite(int id);
        model.Site GetSiteByKey(string key);
        int UpdateGroup(SiteGroupModel siteGroupModel);
        void UpdateSiteFromJson(JObject siteDto);
    }
}