using System;
using DataAccess.EntityFramework.Models.BD.Contact;

namespace DateAccess.Services.ViewModels
{
    public class ContactDTO
    {
        public int Id { get; set; }
        public int SiteId { get; set; }
        public int BusinessTypeId { get; set; }
        public int? ContactPersonId { get; set; }
        public DateTime? NextCall { get; set; }
        public DateTime? LastCall { get; set; }
        public DateTime? NewManagerDate { get; set; }
        public bool DaToCheck { get; set; }
        public string DaToCheckInfo { get; set; }
        public bool ExtManagement { get; set; }
        public bool ReceptionName { get; set; }
        public string Code { get; set; }
        public string Note { get; set; }
        public short? CallFrequency { get; set; }
        public DateTime? CallBackDate { get; set; }
        public ContactPersonDTO ContactPerson { get; set; }
        public BusinessType BusinessType { get; set; }
    }
}
