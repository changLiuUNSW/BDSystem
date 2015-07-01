using System;
using System.Linq;
using DataAccess.EntityFramework.Models.BD.Contact;
using DataAccess.EntityFramework.Models.BD.Lead;
using DataAccess.EntityFramework.Models.BD.Telesale;

namespace DateAccess.Services.ContactService.Call.Scripts.Actions
{
    public class UpdateSendInfo : ScriptAction
    {
        public UpdateSendInfo() : base("Send info", ScriptActionType.UpdateSendInfo)
        {
            
        }

        public override void Update(Contact contact, LeadPersonal person, Telesale telesale)
        {
            if (contact.CallLines != null)
            {
                var callLine = contact.CallLines.Last();
                callLine.EmailInfo = true;
            }
        }
    }
}
