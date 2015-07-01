using DataAccess.EntityFramework.Models.BD.Contact;
using DataAccess.EntityFramework.Models.BD.Lead;
using DataAccess.EntityFramework.Models.BD.Telesale;

namespace DateAccess.Services.ContactService.Call.Scripts.Actions
{
    public class UpdateQualification : ScriptAction
    {
        public UpdateQualification() { }
        public UpdateQualification(string msg) : base(msg, ScriptActionType.UpdateQualification) {}

        public double Number { get; set; }

        public override void Update(Contact contact, LeadPersonal person, Telesale telesale)
        {
            if (contact == null || contact.Site == null)
                return;

            contact.Site.Qualification = Number;
        }
    }
}
