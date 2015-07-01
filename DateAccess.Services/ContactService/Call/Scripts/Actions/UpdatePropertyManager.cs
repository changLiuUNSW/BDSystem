using System;
using System.Linq;
using DataAccess.EntityFramework.Models.BD.Contact;
using DataAccess.EntityFramework.Models.BD.Lead;
using DataAccess.EntityFramework.Models.BD.Telesale;

namespace DateAccess.Services.ContactService.Call.Scripts.Actions
{
    public class UpdatePropertyManager : ScriptAction
    {
        public UpdatePropertyManager():base("Update property manager", ScriptActionType.UpdatePmName) { }
        public UpdatePropertyManager(string desc) : base(desc, ScriptActionType.UpdatePmName)
        {
        }

        public string Company { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Phone { get; set; }

        public override void Update(Contact contact, LeadPersonal person, Telesale telesale)
        {
            var group = contact.Site.Groups.SingleOrDefault(x => x.Type == "Building");

            if (group == null)
                throw new Exception("No property manager building found for this site");

            if (!string.IsNullOrEmpty(Firstname) || !string.IsNullOrEmpty(Lastname))
            {
                group.Firstname = Firstname;
                group.Lastname = Lastname;
            }

            if (!string.IsNullOrEmpty(Phone))
                group.Phone = Phone;

            if (!string.IsNullOrEmpty(Company))
                group.AgentComp = Company;
        }
    }
}
