using System;
using DataAccess.EntityFramework.Models.BD.Contact;
using DataAccess.EntityFramework.Models.BD.Lead;
using DataAccess.EntityFramework.Models.BD.Site;
using DataAccess.EntityFramework.Models.BD.Telesale;

namespace DateAccess.Services.ContactService.Call.Scripts.Actions
{
    public class UpdateCallBack : ScriptAction
    {
        public UpdateCallBack() : base("Call back date", ScriptActionType.UpdateCallBack)
        {
            CallbackDate = DateTime.Today;
        }

        public DateTime CallbackDate { get; set; }

        public override ScriptActionResult Update(Site site, Contact contact, LeadPersonal person, Telesale telesale)
        {
            contact.CallBackDate = CallbackDate;
            return ScriptActionResult.Completed;
        }
    }
}
