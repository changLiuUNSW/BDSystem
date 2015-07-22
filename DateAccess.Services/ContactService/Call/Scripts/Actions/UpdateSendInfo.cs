using System.Linq;
using DataAccess.EntityFramework.Models.BD.Contact;
using DataAccess.EntityFramework.Models.BD.Lead;
using DataAccess.EntityFramework.Models.BD.Site;
using DataAccess.EntityFramework.Models.BD.Telesale;

namespace DateAccess.Services.ContactService.Call.Scripts.Actions
{
    public class UpdateSendInfo : ScriptAction
    {
        public UpdateSendInfo() : base("Send info", ScriptActionType.UpdateSendInfo)
        {
            
        }

        public override ScriptActionResult Update(Site site, Contact contact, LeadPersonal person, Telesale telesale)
        {
            if (contact.CallLines == null)
                return ScriptActionResult.InCompeleted;

            var callLine = contact.CallLines.Last();
            callLine.EmailInfo = true;
            return ScriptActionResult.Completed;
        }
    }
}
