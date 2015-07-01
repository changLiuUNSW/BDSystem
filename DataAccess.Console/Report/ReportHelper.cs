using System;
using System.Linq;
using DataAccess.Common.Contact;
using DataAccess.EntityFramework;
using DataAccess.EntityFramework.Models.BD.Contact;
using DateAccess.Services.ContactService.Reports;
using DateAccess.Services.ContactService.Reports.Config;
using WeeklyReport = DataAccess.EntityFramework.Models.BD.Contact.WeeklyReport;

namespace DataAccess.Console.Report
{
    public class ReportHelper
    {
        private ReportProvider _provider;
        private readonly Random _random;
        private readonly IUnitOfWork _unitOfWork;

        public ReportHelper(IUnitOfWork unitOfWork)
        {
            _provider = new ReportProvider(unitOfWork);
            _random = new Random();
            _unitOfWork = unitOfWork;
        }

        private int Random(int value)
        {
            var plusMinus = _random.Next(1, 10) < 10/2;

            if (value > 0 && !plusMinus)
                return value - _random.Next(1, value);

            return value + _random.Next(1, 10);
        }

        public void GenerateWeeklyHistory(DateTime date)
        {
            var report = _provider.Type(ReportType.Weekly).GetReport().Cast<WeeklySummary>();

            var save = new WeeklyReport
            {
                Date = date,
                Details = report.Select(x=> new WeeklyReportDetail
                {
                    Overdue = Random(x.Overdue),
                    OverdueAndReady = Random(x.OverdueAndReady),
                    Code = x.Code,
                    Total = Random(x.Total)
                }).ToList()
            };
            
            _unitOfWork.WeeklyReportRepository.Add(save);
        }

        public void GenerateFullHistory(DateTime date)
        {
            var report = _provider.Type(ReportType.Complete).GetReport().Cast<CompleteSummary>();

            var save = new FullReport
            {
                Date = date,
                Details = report.Select(x=> new FullReportDetail
                {
                    CallFreq = x.CallFreq,
                    DaToCheck = Random(x.DaToCheck),
                    Code = x.Code,
                    Inhouse = Random(x.Inhouse),
                    NoName = Random(x.NoName),
                    NonInhouse = Random(x.NonInhouse),
                    ReCall = Random(x.ExtManagement),
                    Size = x.Size,
                    Total = Random(x.Total)
                }).ToList()
            };

            _unitOfWork.FullReportRepository.Add(save);
        }
    }
}
