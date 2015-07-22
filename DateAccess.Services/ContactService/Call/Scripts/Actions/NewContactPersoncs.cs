using DataAccess.EntityFramework.Models.BD.Contact;
using DataAccess.EntityFramework.Models.BD.Lead;
using DataAccess.EntityFramework.Models.BD.Site;
using DataAccess.EntityFramework.Models.BD.Telesale;

namespace DateAccess.Services.ContactService.Call.Scripts.Actions
{
    public class UpdateContactName : ScriptAction
    {
        public UpdateContactName():base("New contact", ScriptActionType.UpdateContactName) { }
        public UpdateContactName(string msg) : base(msg, ScriptActionType.UpdateContactName) {}

        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public override ScriptActionResult Update(Site site, Contact contact, LeadPersonal person, Telesale telesale)
        {
            if (string.IsNullOrEmpty(Firstname) && string.IsNullOrEmpty(Lastname))
                return ScriptActionResult.InCompeleted;

            if (contact == null || contact.ContactPerson == null)
                return ScriptActionResult.InCompeleted;

            contact.ContactPerson.Firstname = Firstname;
            contact.ContactPerson.Lastname = Lastname;
            return ScriptActionResult.Completed;
        }
    }
}
