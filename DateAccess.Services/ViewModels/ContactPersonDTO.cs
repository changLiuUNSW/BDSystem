namespace DateAccess.Services.ViewModels
{
    public class ContactPersonDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Position { get; set; }
        public string Mobile { get; set; }
        public string Fax { get; set; }
        public string DirectLine { get; set; }
        public string PoNumber { get; set; }
        public string PoUnit { get; set; }
        public string PoStreet { get; set; }
        public string PoSuburb { get; set; }
        public string PoState { get; set; }
        public string PoPostcode { get; set; }

        public string SiteName { get; set; }
        public int SiteId { get; set; }
    }
}
