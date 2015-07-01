using DataAccess.EntityFramework;
using DateAccess.Services.ContactService.Reports.Base;
using DateAccess.Services.ContactService.Reports.Config;
using DateAccess.Services.ContactService.Reports.Types;

namespace DateAccess.Services.ContactService.Reports
{
    public class ReportProvider
    {
        private readonly IUnitOfWork _unitOfWork;

        internal ReportProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Report Type(ReportType type)
        {
            switch (type)
            {
                case ReportType.Assignable:
                    return new AssignableReport(_unitOfWork);
                case ReportType.Weekly:
                    return new WeeklyReport(_unitOfWork);
                case ReportType.Complete:
                    return new CompleteReport(_unitOfWork);
                default:
                    return new CompleteReport(_unitOfWork);
            }
        }
    }
}
