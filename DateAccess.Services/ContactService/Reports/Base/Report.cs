using System.Collections.Generic;
using DataAccess.Common.Contact;
using DataAccess.EntityFramework;

namespace DateAccess.Services.ContactService.Reports.Base
{
    /// <summary>
    /// base class for all report factory
    /// </summary>
    public abstract class Report
    {
        protected readonly IUnitOfWork UnitOfWork;
        protected Report(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public abstract IReadOnlyList<ContactSummary> GetReport();
        public abstract IReadOnlyList<ContactSummary> GetReport(IEnumerable<string> targets);
        public abstract IReadOnlyList<ContactSummary> GetHistory();
    }
}
