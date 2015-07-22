using System;
using DataAccess.EntityFramework.Models.BD.Contact;
using DataAccess.EntityFramework.Models.BD.Lead;
using DataAccess.EntityFramework.Models.BD.Site;
using DataAccess.EntityFramework.Models.BD.Telesale;

namespace DateAccess.Services.ContactService.Call.Scripts.Actions
{
    /// <summary>
    /// caution on changing the type value because some of them are used in angularjs as a reference
    /// </summary>
    public enum ScriptActionType
    {
        CreateCleaningLead = 1,
        CreateMaintenanceLead = 2,
        NewTaskForDH = 3,
        NewPropertyManager = 4,
        UpdateContactName = 5,
        UpdateNextCallDate = 11,
        UpdateExtManagement = 12,
        UpdateEmail = 14,
        UpdateQualification = 15,
        UpdateSecurityPerson = 17,
        UpdateDaCheck = 19,
        UpdateCallBack = 20,
        UpdateGroup = 21,
        UpdateTenant = 22,
        UpdateSendInfo = 23
    }

    [Flags]
    public enum ScriptActionResult
    {
        Completed = 1,
        InCompeleted = Completed << 1,
        EmailRequired = Completed << 1
    }

    public abstract class ScriptAction
    {
        protected ScriptAction() { }
        protected ScriptAction(string desc, ScriptActionType type)
        {
            Description = desc;
            Type = type;
        }

        public string Description { get; set; }
        public ScriptActionType Type { get; set; }

        public abstract ScriptActionResult Update(Site site, Contact contact, LeadPersonal person, Telesale telesale);
    }
}
