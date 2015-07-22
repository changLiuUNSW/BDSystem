using System;
using System.Linq;
using DataAccess.EntityFramework.Models.BD.Contact;
using DataAccess.EntityFramework.Models.BD.Lead;
using DataAccess.EntityFramework.Models.BD.Site;
using DataAccess.EntityFramework.Models.BD.Telesale;
using DataAccess.EntityFramework.TypeLibrary;

namespace DateAccess.Services.ContactService.Call.Scripts.Actions
{
    public class UpdateTenant : ScriptAction
    {
        public UpdateTenant() : base("Update tenant as OPR", ScriptActionType.UpdateTenant) { }

        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }

        public override ScriptActionResult Update(Site site, Contact contact, LeadPersonal person, Telesale telesale)
        {
            if (site == null)
                throw new Exception("This site cant not be changed to OPR, please contact DB admin");

            if (site.Contacts.Any(x=>x.Code == "OPR"))
                throw new Exception("Not a valid tenant, the site already has an OPR contact");

            site.PropertyManaged = false;
            site.TsToCall = true;
            site.Size = Size.Size025.ToString();

            contact.DaToCheck = true;
            contact.DaToCheckInfo = "Need new tenant for PMS"; 
             

            site.Contacts.Add(new Contact
            {
                SiteId = site.Id,
                BusinessTypeId = (int)BusinessTypes.Cleaning,
                LastCall = DateTime.Today,
                NextCall = DateTime.Today.AddDays(contact.CallFrequency.GetValueOrDefault() * 7),
                Code = "OPR",
                CallFrequency = 3,
                ContactPerson = new ContactPerson
                {
                    SiteId = site.Id,
                    Lastname = Lastname,
                    Firstname = Firstname,
                    DirectLine = Phone,
                    Mobile = Mobile,
                    CreateDate = DateTime.Today
                }
            });
            
            return ScriptActionResult.Completed;
        }
    }
}
