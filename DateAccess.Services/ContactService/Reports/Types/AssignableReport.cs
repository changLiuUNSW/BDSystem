using System;
using System.Collections.Generic;
using System.Linq;
using DataAccess.Common.Contact;
using DataAccess.EntityFramework;
using DateAccess.Services.ContactService.Reports.Base;
using DateAccess.Services.ContactService.Reports.Config;

namespace DateAccess.Services.ContactService.Reports.Types
{
    internal class AssignableReport : Report
    {
        internal AssignableReport(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public override IReadOnlyList<ContactSummary> GetReport()
        {
            var codes = Enum.GetNames(typeof (AssignablePmCodes));

            var contacts = UnitOfWork.ContactRepository.AssignableSummary();

            return UnitOfWork.ContactRepository.AssignableSummary(codes).Concat(contacts).ToList();
        }

        public override IReadOnlyList<ContactSummary> GetReport(IEnumerable<string> targets)
        {
            throw new NotImplementedException();
        }

        public override IReadOnlyList<ContactSummary> GetHistory()
        {
            throw new NotImplementedException();
        }
    }
}
