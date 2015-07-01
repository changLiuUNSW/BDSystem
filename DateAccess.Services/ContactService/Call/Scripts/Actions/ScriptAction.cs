using DataAccess.EntityFramework.Models.BD.Contact;
using DataAccess.EntityFramework.Models.BD.Lead;
using DataAccess.EntityFramework.Models.BD.Telesale;

namespace DateAccess.Services.ContactService.Call.Scripts.Actions
{
    /// <summary>
    /// caution on changing the type value because they are used in angularjs as a reference
    /// </summary>
    public enum ScriptActionType
    {
        CreateCleaningLead = 1,
        CreateMaintenanceLead = 2,
        UpdateNextCallDate = 11,
        UpdateExtManagement = 12,
        UpdateContactName = 13,
        UpdateEmail = 14,
        UpdateQualification = 15,
        UpdatePmName = 16,
        UpdateSecurityPerson = 17,
        TaskForDH = 18,
        UpdateDaCheck = 19,
        UpdateCallBack = 20,
        UpdateGroup = 21,
        UpdateTenant = 22,
        UpdateSendInfo = 23
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

        public abstract void Update(Contact contact, LeadPersonal person, Telesale telesale);
    }
}
