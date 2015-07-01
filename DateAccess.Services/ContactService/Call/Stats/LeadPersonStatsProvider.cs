using System.Collections.Generic;
using System.Linq;
using DataAccess.Common.Contact;
using DataAccess.EntityFramework;
using DataAccess.EntityFramework.Extensions.Utilities;
using DataAccess.EntityFramework.Models.BD.Lead;
using DateAccess.Services.ContactService.Call.Models;

namespace DateAccess.Services.ContactService.Call.Stats
{
    /// <summary>
    /// calculate how much contacts needed per week/year for each GM/GGM/BD/ROP
    /// </summary>
    internal class LeadPersonStatsProvider : ILeadPersonStatsProvider
    {
        private readonly IUnitOfWork _unitOfWork;
        private LeadShiftInfo _leadShiftInfo;

        public LeadPersonStatsProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private LeadShiftInfo LeadShiftInfo
        {
            get
            {
                if (_leadShiftInfo == null)
                    _leadShiftInfo = _unitOfWork.LeadShiftInfoRepository.Get().SingleOrDefault();

                return _leadShiftInfo;
            }
        }

        public IList<LeadPersonStat> GetStats(IList<LeadPersonal> persons)
        {
            var stats = new List<LeadPersonStat>();
            var summary = _unitOfWork.ContactRepository.AllocationSummary().ToList();

            foreach (var person in persons)
            {
                var id = person.Id;
                var sums = summary.Where(x => x.PersonId == id).ToList();

                var stat = new LeadPersonStat
                {
                    PersonId = id,
                    Weekly = WeeklyStats(sums),
                    Yearly = YearlyStats(sums)
                };

                if (LeadShiftInfo != null)
               {
                    stat.Weekly.Needed = person.GetWeeklyContactNeeds(LeadShiftInfo);
                    stat.Yearly.Needed = person.GetYearlyContactNeeds(LeadShiftInfo);                
                }

                stats.Add(stat);
            }

            return stats;
        }

        private LeadPersonStat.Stat GetStats(IList<AllocationSummary> summaries)
        {
            var sunday = DateExtension.Sunday();

            var thisWeekSum = summaries.Where(x => x.NextCall <= sunday).ToList();

            return new LeadPersonStat.Stat
            {
                Inhouse = thisWeekSum.Sum(x => x.Inhouse),
                NotInhouse = thisWeekSum.Sum(x => x.NonInhouse)
            };
        }

        private LeadPersonStat.Stat WeeklyStats(IList<AllocationSummary> summaries)
        {
            var stat = GetStats(summaries);
            stat.Total = stat.Inhouse + stat.NotInhouse;
            return stat;
        }

        private LeadPersonStat.Stat YearlyStats(IList<AllocationSummary> summaries)
        {
            var stat = GetStats(summaries);
            stat.Total = stat.NotInhouse *CallsPerYear() + stat.Inhouse;
            return stat;
        }

        private int CallsPerYear()
        {
            return 52/LeadShiftInfo.CallCycleWeeks;
        }
    }
}
