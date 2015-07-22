using System;

namespace DateAccess.Services.ViewModels
{
    public class CleaningContractDTO
    {
        public int SiteId { get; set; }
        public string Contractor { get; set; }
        public string ContactDuringQuote { get; set; }
        public decimal PricePa { get; set; }
        public string UnsuccessReason { get; set; }
        public DateTime? ReviewDate { get; set; }
        public DateTime? DateQuoted { get; set; }
        public double? CleaningFreq { get; set; }
        public double? QualifyingQuantity { get; set; }
    }
}
