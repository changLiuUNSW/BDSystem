using System;

namespace DataAccess.Common.SearchModels
{
    public abstract class QuoteModelBase
    {
        public int Id { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public string QuoteType { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public string Phone { get; set; }
        public string Company { get; set; }
        public string Unit { get; set; }
        public string Number { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Street { get; set; }
        public string Suburb { get; set; }
        public string State { get; set; }
        public string Postcode { get; set; }
        public string QpInitial { get; set; }
        public string QpName { get; set; }
        public decimal? LastestPA { get; set; }
        public decimal? TotalPA { get; set; }
        public decimal? TotalPW { get; set; }
        public DateTime? LastContactDate { get; set; }
    }


    public class QuoteProgressModel : QuoteModelBase
    {
        public bool Hidden { get; set; }
    }

    public class QuoteCurrentModel : QuoteModelBase
    {
        public bool ContactCheckOverDue { get; set; }
        public bool DeadCheckOverDue { get; set; }
        public bool AjustCheckOverDue { get; set; }
    }
}