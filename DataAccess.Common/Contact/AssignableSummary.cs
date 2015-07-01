namespace DataAccess.Common.Contact
{
    public class AssignableSummary : ContactSummary
    {
        public object Area { get; set; }
        public int Inhouse { get; set; }
        public int NonInhouse { get; set; }
        public int Total { get; set; }
        public string LeadPerson { get; set; }
        public string Size { get; set; }
    }
}
