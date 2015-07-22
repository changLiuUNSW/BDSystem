namespace DateAccess.Services.Events
{
    public class NewLead : IDomainEvent
    {
        public string Name { get; set; }
        public int LeadId { get; set; }
        public string LeadType { get; set; }
        public string Company { get; set; }
        public string Phone { get; set; }
        public string Unit { get; set; }
        public string Number { get; set; }
        public string Street { get; set; }
        public string Suburb { get; set; }
        public string State { get; set; }
        public string Postcode { get; set; }
        public string Url { get; set; }
        public string Email { get; set; }
    }
}
