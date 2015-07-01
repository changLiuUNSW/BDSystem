namespace DataAccess.Common.Contact
{
    public class WeeklySummary : ContactSummary
    {
        public string Size { get; set; }
        public string Zone { get; set; }
        public int Overdue { get; set; }
        public int OverdueAndReady { get; set; }
        public int Total { get; set; }
    }
}
