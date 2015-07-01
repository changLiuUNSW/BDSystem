using DataAccess.EntityFramework.Models.BD.Contact;
using DataAccess.EntityFramework.Models.BD.Lead;
using DataAccess.EntityFramework.Models.BD.Telesale;

namespace DateAccess.Services.ContactService.Call.Scripts.Actions
{
    public class UpdateDaCheck : ScriptAction
    {
        public UpdateDaCheck() : base("Da to check", ScriptActionType.UpdateDaCheck)
        {
            
        }

        public string InfoForDa { get; set; }

        public override void Update(Contact contact, LeadPersonal person, Telesale telesale)
        {
            contact.DaToCheck = true;
            contact.DaToCheckInfo = InfoForDa;
        }
    }
}
