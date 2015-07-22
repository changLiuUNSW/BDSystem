using DataAccess.EntityFramework.Models.BD.Contact;
using DataAccess.EntityFramework.Models.BD.Lead;
using DataAccess.EntityFramework.Models.BD.Site;
using DataAccess.EntityFramework.Models.BD.Telesale;

namespace DateAccess.Services.ContactService.Call.Scripts.Actions
{
    public class UpdateEmail : ScriptAction
    {
        public UpdateEmail() : base("Update email address.", ScriptActionType.UpdateEmail) {}

        public string Email { get; set; }

        public override ScriptActionResult Update(Site site, Contact contact, LeadPersonal person, Telesale telesale)
        {
            if (contact == null || contact.ContactPerson == null)
                return ScriptActionResult.InCompeleted;

            contact.ContactPerson.Email = Email;
            return ScriptActionResult.Completed;
        }
    }
}
