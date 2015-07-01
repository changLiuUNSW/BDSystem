namespace DataAccess.Common
{
    public class ApplicationSettings
    {

        public string BugEmail { get; set; }
        public string SMTPServer { get; set; }
        public string SMTPUser { get; set; }
        public string SMTPPassword { get; set; }
        public string FromEmail { get; set; }
        public string AdminEmail { get; set; }
        public string WPEmail { get; set; }
        public string TestingOverrideEmail { get; set; }
        public string BDGMInitial { get; set; }
        public string TempFolder { get; set; }
        public string NewLeadAlertEmailPath { get; set; }
        public string QuoteMergedEmailPath { get; set; }
        public string BDGMEmail { get; set; }
        public string QuoteFinalReviewEmail { get; set; }
        public string QuotePrintEmailPath { get; set; }
        public string QuotePresentClientEmailPath { get; set; }
        public string QuoteDocumentEmailPath { get; set; }
        public string LeadAppointmentEmailPath { get; set; }
        public string QuoteCostsEmailPath { get; set; }
        public string QuoteIssueAlertEmailPath { get; set; }
        public string QuoteUploadPath { get; set; }
        public string CostUploadPath { get; set; }
        public string SecurityWorkBookPath { get; set; }
        public string NzWorkBookPath { get; set; }
    }
}