namespace DataAccess.Common.Contact
{
    public class CompleteSummary : ContactSummary
    {
        public int? CallFreq { get; set; }
        public string Size { get; set; }
        public int DaToCheck { get; set; }
        public int NoName { get; set; }
        public int ExtManagement { get; set; }
        public int Inhouse { get; set; }
        public int NonInhouse { get; set; }
        public int Total { get; set; }
    }
}
