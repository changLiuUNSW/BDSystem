using System.Collections.Generic;
using System.Linq;
using DataAccess.EntityFramework;
using DataAccess.EntityFramework.Models.BD.Lead;
using DateAccess.Services.ContactService.Call.Stats;

namespace DateAccess.Services.ContactService
{
    public class LeadPriorityName
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Initial { get; set; }
    }

    public interface ILeadPersonalService : IRepositoryService<LeadPersonal>
    {
        int UpdateShiftInfo(LeadShiftInfo leadShiftInfo);
        int InsertOrUpdate(IList<LeadPersonal> personals);
        LeadShiftInfo LeadShiftInfo { get; }
        IList<LeadGroup> GetLeadGroup(params string[] groups);
        ILeadPersonStatsProvider StatsProvider { get;}
        IList<LeadPriorityName> GetNames();
        IList<LeadShiftInfo> GetLeadShift();
    }

    internal class LeadPersonalService : RepositoryService<LeadPersonal>, ILeadPersonalService
    {
        private LeadShiftInfo _leadShiftInfo;

        public LeadPersonalService(IUnitOfWork unitOfWork) :base(unitOfWork)
        {
            StatsProvider = new LeadPersonStatsProvider(unitOfWork);
        }

        public LeadShiftInfo LeadShiftInfo
        {
            get { return _leadShiftInfo ?? (_leadShiftInfo = UnitOfWork.LeadShiftInfoRepository.Get().Single()); }
            private set { _leadShiftInfo = value; }
        }

        public ILeadPersonStatsProvider StatsProvider { get; private set; }


        public IList<LeadPriorityName> GetNames()
        {
            return UnitOfWork.LeadPersonalRepository.Get(x => new LeadPriorityName
            {
                Id = x.Id,
                Name = x.Name,
                Initial = x.Initial
            });
        }

        public IList<LeadGroup> GetLeadGroup(params string[] groups)
        {
            if (groups.Any())
                return UnitOfWork.LeadGroupRepository.Get(x => groups.Contains(x.Group));

            return UnitOfWork.LeadGroupRepository.Get();
        }

        public IList<LeadShiftInfo> GetLeadShift()
        {
            return UnitOfWork.LeadShiftInfoRepository.Get();
        }

        public int InsertOrUpdate(IList<LeadPersonal> personals)
        {
            foreach (var personal in personals)
            {
                if (personal.Id == 0)
                    UnitOfWork.LeadPersonalRepository.Add(personal);
                else
                    UnitOfWork.LeadPersonalRepository.Update(personal);
            }

            return UnitOfWork.Save();
        }

        public int UpdateShiftInfo(LeadShiftInfo leadShiftInfo)
        {
            UnitOfWork.LeadShiftInfoRepository.Update(leadShiftInfo);
            LeadShiftInfo = leadShiftInfo;
            return UnitOfWork.Save();
        }
    }
}
