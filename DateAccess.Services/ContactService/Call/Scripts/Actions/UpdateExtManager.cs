using DataAccess.EntityFramework.Models.BD.Contact;
using DataAccess.EntityFramework.Models.BD.Lead;
using DataAccess.EntityFramework.Models.BD.Site;
using DataAccess.EntityFramework.Models.BD.Telesale;

namespace DateAccess.Services.ContactService.Call.Scripts.Actions
{
    public class UpdateExtManager : ScriptAction
    {
        public UpdateExtManager() : base("Ext Management", ScriptActionType.UpdateExtManagement)
        {
        }

        public string InfoForDa { get; set; }

        public override ScriptActionResult Update(Site site, Contact contact, LeadPersonal person, Telesale telesale)
        {
            contact.ExtManagement = true;
            contact.DaToCheckInfo = InfoForDa;
            return ScriptActionResult.Completed;
        }
    }
}
