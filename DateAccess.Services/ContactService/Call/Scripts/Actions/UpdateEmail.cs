using DataAccess.EntityFramework.Models.BD.Contact;
using DataAccess.EntityFramework.Models.BD.Lead;
using DataAccess.EntityFramework.Models.BD.Telesale;

namespace DateAccess.Services.ContactService.Call.Scripts.Actions
{
    public class UpdateEmail : ScriptAction
    {
        public UpdateEmail() : base("Update email address.", ScriptActionType.UpdateEmail) {}

        public string Email { get; set; }

        public override void Update(Contact contact, LeadPersonal person, Telesale telesale)
        {
            if (contact == null || contact.ContactPerson == null)
                return;

            contact.ContactPerson.Email = Email;
        }
    }
}
