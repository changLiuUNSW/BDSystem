using System;
using System.Linq;
using DataAccess.EntityFramework.Models.BD.Contact;
using DataAccess.EntityFramework.Models.BD.Lead;
using DataAccess.EntityFramework.Models.BD.Telesale;
using DataAccess.EntityFramework.TypeLibrary;

namespace DateAccess.Services.ContactService.Call.Scripts.Actions
{
    public class UpdateTenant : ScriptAction
    {
        public UpdateTenant() : base("Update tenant as OPR", ScriptActionType.UpdateTenant) { }

        public int TenantId { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }

        public override void Update(Contact contact, LeadPersonal person, Telesale telesale)
        {
            if (TenantId <= 0)
                throw new Exception("Invalid tenant id");

            var building = contact.Site.Groups.SingleOrDefault(x => x.Type == "Building");

            if (building == null)
                throw new Exception("Tenant building not found");

            var tenant = building.Sites.SingleOrDefault(x => x.Id == TenantId);

            if (tenant == null)
                throw new Exception("Tenant not found");

            if (tenant.Contacts.Any(x=>x.Code == "OPR"))
                throw new Exception("Not a valid tenant, the site already has an OPR contact");

            tenant.PropertyManaged = false;
            tenant.TsToCall = true;
            tenant.Size = Size.Size025.ToString();

            contact.DaToCheck = true;
            contact.DaToCheckInfo = "Need new tenant for PMS"; 
             

            tenant.Contacts.Add(new Contact
            {
                SiteId = tenant.Id,
                BusinessTypeId = (int)BusinessTypes.Cleaning,
                LastCall = DateTime.Today,
                NextCall = DateTime.Today.AddDays(contact.CallFrequency.GetValueOrDefault() * 7),
                Code = "OPR",
                CallFrequency = 3,
                ContactPerson = new ContactPerson
                {
                    SiteId = tenant.Id,
                    Lastname = Lastname,
                    Firstname = Firstname,
                    DirectLine = Phone,
                    Mobile = Mobile,
                    CreateDate = DateTime.Today
                }
            });
        }
    }
}
