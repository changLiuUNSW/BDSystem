using System;
using System.Linq;
using DataAccess.EntityFramework.Models.BD.Contact;
using DataAccess.EntityFramework.Models.BD.Lead;
using DataAccess.EntityFramework.Models.BD.Site;
using DataAccess.EntityFramework.Models.BD.Telesale;

namespace DateAccess.Services.ContactService.Call.Scripts.Actions
{
    public class NewPropertyManager : ScriptAction
    {
        public NewPropertyManager():base("New property manager", ScriptActionType.NewPropertyManager) { }
        public NewPropertyManager(string desc) : base(desc, ScriptActionType.NewPropertyManager)
        {
        }

        public string Company { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Phone { get; set; }

        public override ScriptActionResult Update(Site site, Contact contact, LeadPersonal person, Telesale telesale)
        {
            var group = contact.Site.Groups.SingleOrDefault(x => x.Type == "Building");
            if (group == null)
                throw new Exception("No property manager building found for this site");


            throw new NotImplementedException();
            //todo need to re-do on this
            /*if (!string.IsNullOrEmpty(Firstname) || !string.IsNullOrEmpty(Lastname))
            {
                group.Firstname = Firstname;
                group.Lastname = Lastname;
            }

            if (!string.IsNullOrEmpty(Phone))
                group.Phone = Phone;

            if (!string.IsNullOrEmpty(Company))
                group.AgentComp = Company;*/
        }
    }
}
