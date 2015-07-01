using System;
using DataAccess.EntityFramework.Models.BD.Contact;
using DataAccess.EntityFramework.Models.BD.Lead;
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

        public override void Update(Contact contact, LeadPersonal person, Telesale telesale)
        {
            contact.CallBackDate = CallbackDate;
        }
    }
}
