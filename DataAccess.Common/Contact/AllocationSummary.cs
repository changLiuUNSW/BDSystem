using System;

namespace DataAccess.Common.Contact
{
    public class AllocationSummary : ContactSummary
    {
        public int PersonId { get; set; }
        public string Initial { get; set; }
        public DateTime? NextCall { get; set; }
        public int Inhouse { get; set; }
        public int NonInhouse { get; set; }
        public int Total { get; set; }
    }
}
