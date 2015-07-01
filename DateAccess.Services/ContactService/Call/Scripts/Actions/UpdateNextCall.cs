using System;
using DataAccess.EntityFramework.Models.BD.Contact;
using DataAccess.EntityFramework.Models.BD.Lead;
using DataAccess.EntityFramework.Models.BD.Telesale;
using DateAccess.Services.ContactService.Call.Scripts.Info;

namespace DateAccess.Services.ContactService.Call.Scripts.Actions
{
    public class UpdateNextCall : ScriptAction
    {
        public UpdateNextCall()
            : base(
                string.Format("Next call in {0} months.",
                Replaceable.String[ReplaceType.UpdateInterval]),
                ScriptActionType.UpdateNextCallDate)
        {
            UpdateInMonth = true;
        }

        public UpdateNextCall(bool updateInMonth)
        {
            UpdateInMonth = updateInMonth;

            if (UpdateInMonth)
            {
                Description = string.Format("Next call in {0} month.",
                Replaceable.String[ReplaceType.UpdateInterval]);
            }
            else
            {
                Description = "Next call in 3 three weeks.";
            }
            
            Type = ScriptActionType.UpdateNextCallDate;
        }

        public Boolean UpdateInMonth { get; set; }

        public override void Update(Contact contact, LeadPersonal person, Telesale telesale)
        {
            if (contact != null && contact.CallFrequency.HasValue)
            {
                if (UpdateInMonth)
                    contact.NextCall = DateTime.Today.AddMonths(contact.CallFrequency.Value);
                else
                    contact.NextCall = DateTime.Today.AddDays(3*7);
            }
        }
    }
}
