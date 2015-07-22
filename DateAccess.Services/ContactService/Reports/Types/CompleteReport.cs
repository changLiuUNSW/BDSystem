using System;
using System.Collections.Generic;
using System.Linq;
using DataAccess.Common.Contact;
using DataAccess.EntityFramework;
using DataAccess.EntityFramework.Extensions;

namespace DateAccess.Services.ContactService.Reports.Types
{
    internal class CompleteReport : Base.Report
    {
        internal CompleteReport(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            
        }

        public override IReadOnlyList<ContactSummary> GetReport()
        {
            return UnitOfWork.ContactRepository.CompleteSummary().ToList();
        }

        public override IReadOnlyList<ContactSummary> GetReport(IEnumerable<string> targets)
        {
            throw new NotImplementedException();
        }

        public override IReadOnlyList<ContactSummary> GetHistory()
        {
            var lastSunday = DateExtension.Sunday().AddDays(-7);
            var lastMonday = DateExtension.Monday().AddDays(-7);

            var history = UnitOfWork.FullReportRepository.SingleOrDefault(x => x.Date >= lastMonday && x.Date <= lastSunday);

            if (history == null || history.Details == null)
                return null;

            return history.Details.ToList();
        }
    } 
}
