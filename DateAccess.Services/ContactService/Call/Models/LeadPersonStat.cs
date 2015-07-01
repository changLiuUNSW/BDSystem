namespace DateAccess.Services.ContactService.Call.Models
{
    public class LeadPersonStat
    {
        public class Stat
        {
            public int Inhouse { get; set; }
            public int NotInhouse { get; set; }
            public int Needed { get; set; }
            public int Total { get; set; }
        }

        public int PersonId { get; set; }
        public Stat Weekly { get; set; }
        public Stat Yearly { get; set; }
    }
}
