using System.Collections.Generic;
using DataAccess.EntityFramework;

namespace DateAccess.Services.StatisticService
{
    public class SummaryCount
    {
        public string Type { get; set; }
        public int TotalCount { get; set; }
    }
    internal class SummaryService : ISummaryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SummaryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<SummaryCount> GetContactPersonCount()
        {
            var countMap = new List<SummaryCount>
            {
                new SummaryCount
                {
                    Type = "cleaning",
                    TotalCount=_unitOfWork.ContactRepository.Count(l=>l.ContactPersonId!=null&&l.BusinessType.Type=="Cleaning")
                },
                new SummaryCount
                {
                     Type = "security",
                    TotalCount=_unitOfWork.ContactRepository.Count(l=>l.ContactPersonId!=null&&l.BusinessType.Type=="Security")
                },
                  new SummaryCount
                {
                     Type = "maintenance",
                    TotalCount=_unitOfWork.ContactRepository.Count(l=>l.ContactPersonId!=null&&l.BusinessType.Type=="Maintenance")
                },
                new SummaryCount
                {
                     Type = "other",
                     TotalCount=_unitOfWork.ContactPersonRepository.Count(l=>l.Contacts.Count==0)
                }
            };
            return countMap;
        }

        public List<SummaryCount> GetSiteCount()
        {
            var countMap = new List<SummaryCount>
            {
                new SummaryCount
                {
                    Type = "cleaning",
                    TotalCount=_unitOfWork.ContactRepository.Count(l=>l.BusinessType.Type=="Cleaning")
                },
                new SummaryCount
                {
                     Type = "security",
                    TotalCount=_unitOfWork.ContactRepository.Count(l=>l.BusinessType.Type=="Security")
                },
                new SummaryCount
                {
                    Type = "maintenance",
                    TotalCount=_unitOfWork.ContactRepository.Count(l=>l.BusinessType.Type=="Maintenance")
                },
                new SummaryCount
                {
                    Type = "group",
                    TotalCount=_unitOfWork.SiteGroupRepository.Count()
                }
            };
            return countMap;
        }
    }


    public interface ISummaryService
    {
        List<SummaryCount> GetContactPersonCount();
        List<SummaryCount> GetSiteCount();
    }
}
