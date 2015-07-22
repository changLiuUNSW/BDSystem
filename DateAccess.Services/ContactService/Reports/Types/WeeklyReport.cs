using System;
using System.Collections.Generic;
using System.Linq;
using DataAccess.Common.Contact;
using DataAccess.EntityFramework;
using DataAccess.EntityFramework.Extensions;
using DataAccess.EntityFramework.TypeLibrary;
using DateAccess.Services.ContactService.Reports.Comparer;
using DateAccess.Services.ContactService.Reports.Config;

namespace DateAccess.Services.ContactService.Reports.Types
{
    /// <summary>
    /// general purpose report
    /// </summary>
    internal class WeeklyReport : Base.Report
    {
        private readonly IList<string> _groupBd;
        private readonly IList<string> _groupPm;
        private readonly IList<string> _groupGrp;
        private readonly IList<string> _groupOpr; 

        internal WeeklyReport(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _groupBd = Enum.GetNames(typeof(WklReportCodeBd));
            _groupPm = Enum.GetNames(typeof(WklReportCodePm));
            _groupGrp = Enum.GetNames(typeof(WklReportCodeGrp));
            _groupOpr = Enum.GetNames(typeof(WklReportCodeOpr));
        }

        /// <summary>
        /// general weekly report for all qp codes
        /// </summary>
        /// <returns></returns>
        public override IReadOnlyList<ContactSummary> GetReport()
        {
            var list = _groupBd.Concat(_groupPm).Concat(_groupGrp).Concat(_groupOpr);
            var summaries = GetReport(list);
            return summaries.OrderByDescending(x => _groupBd.Contains(x.Code, StringComparer.InvariantCultureIgnoreCase))
                    .ThenByDescending(x => _groupPm.Contains(x.Code, StringComparer.InvariantCultureIgnoreCase))
                    .ThenByDescending(x => _groupGrp.Contains(x.Code, StringComparer.InvariantCultureIgnoreCase))
                    .ThenBy(x=>x.Code)
                    .ToList();
        }

        /// <summary>
        /// get report base on the targeted qp codes
        /// </summary>
        /// <param name="targets"></param>
        /// <returns></returns>
        public override IReadOnlyList<ContactSummary> GetReport(IEnumerable<string> targets)
        {
            var summaries = UnitOfWork.ContactRepository.WeeklySummary(targets);
            return GetNonOprSummary(summaries).Concat(GetOprSummary(summaries)).ToList();
        }

        public override IReadOnlyList<ContactSummary> GetHistory()
        {
            var lastSunday = DateExtension.Sunday().AddDays(-7);
            var lastMonday = DateExtension.Monday().AddDays(-7);

            var history = UnitOfWork.WeeklyReportRepository.SingleOrDefault(x=>x.Date >= lastMonday && x.Date <= lastSunday);

            if (history == null || history.Details == null)
                return null;

            return history.Details.ToList();
        }

        /// <summary>
        /// all summaries except OPR
        /// </summary>
        /// <param name="summaries"></param>
        /// <returns></returns>
        private IEnumerable<WeeklySummary> GetNonOprSummary(IEnumerable<WeeklySummary> summaries)
        {
            var filtered =
                summaries.Where(
                    x =>
                        !string.Equals(x.Code, WklReportCodeOpr.OPR.ToString(),
                            StringComparison.InvariantCultureIgnoreCase));

            return filtered.GroupBy(x => x.Code).Select(x => new WeeklySummary
            {
                Code = x.Key,
                Overdue = x.Sum(y => y.Overdue),
                OverdueAndReady = x.Sum(y => y.OverdueAndReady),
                Total = x.Sum(y => y.Total)
            }).ToList();
        }

        /// <summary>
        /// opr contact summary for size 120
        /// </summary>
        /// <param name="summaries"></param>
        /// <returns></returns>
        private IEnumerable<WeeklySummary> GetOprSummary(IEnumerable<WeeklySummary> summaries)
        {
            var size = Size.Size120.GetDescription();
            var allocations = UnitOfWork.AllocationRepository.Get(x => x.Size == size);

            var list = summaries.Where(x => string.Equals(x.Code, WklReportCodeOpr.OPR.ToString(),
                StringComparison.InvariantCultureIgnoreCase) &&
                                            string.Equals(x.Size, size, StringComparison.InvariantCultureIgnoreCase));

            foreach (var allocation in allocations.GroupBy(x=>x.Initial))
            {
                var allocationAddress = allocation.Select(x => new WeeklySummary
                {
                    Size = x.Size,
                    Zone = x.Zone
                });

                var allocatedSummaries = list.Intersect(allocationAddress, new WeeklySummaryComparer());

                yield return new WeeklySummary
                {
                    Code = allocation.Key,
                    Overdue = allocatedSummaries.Sum(x => x.Overdue),
                    OverdueAndReady = allocatedSummaries.Sum(x => x.OverdueAndReady),
                    Total = allocatedSummaries.Sum(x => x.Total)
                };
            }
        }
    }
}
