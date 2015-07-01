using System;

namespace DateAccess.Services.ViewModels
{
    public class ContactPersonHistoryDTO
    {
        public int Id { get; set; }
        public int OriginalContactPersonId { get; set; }
        public string ReasonForChange { get; set; }
        public string SiteKey { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string CurrentFirstName { get; set; }
        public string CurrentLastName { get; set; }
        public DateTime Time { get; set; }
    }
}