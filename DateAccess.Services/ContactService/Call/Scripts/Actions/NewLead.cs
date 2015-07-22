using DataAccess.EntityFramework.Models.BD.Contact;
using DataAccess.EntityFramework.Models.BD.Lead;
using DataAccess.EntityFramework.Models.BD.Site;
using DataAccess.EntityFramework.Models.BD.Telesale;
using DataAccess.EntityFramework.TypeLibrary;
using DateAccess.Services.ContactService.Call.Models;
using DateAccess.Services.ContactService.Leads;

namespace DateAccess.Services.ContactService.Call.Scripts.Actions
{
    public class NewLead : ScriptAction
    {
        public NewLead(): base("New lead", ScriptActionType.CreateCleaningLead) { Init(); }
        public NewLead(string msg, ScriptActionType type) : base(msg, type) { Init(); }

        public string Phone { get; set; }
        public string Address { get; set; }

        private LeadAssembler LeadAssembler { get; set; }

        private void Init()
        {
            Phone = null;
            Address = null;
            LeadAssembler = new LeadAssembler {LeadFactory = new LeadFactory()};
        }

        public override ScriptActionResult Update(Site site, Contact contact, LeadPersonal person, Telesale telesale)
        {
            Lead lead; 

            LeadAssembler.Contact = contact;
            LeadAssembler.LeadPersonal = person;
            LeadAssembler.Telesale = telesale;

            switch (Type)
            {
                case ScriptActionType.CreateMaintenanceLead:
                    lead = LeadAssembler.Assemble(BusinessTypes.Maintenance);
                    break;
                default:
                    lead = LeadAssembler.Assemble(BusinessTypes.Cleaning);
                    break;
            }

            contact.Leads.Add(lead);

            return ScriptActionResult.Completed | ScriptActionResult.EmailRequired;
        }
    }
}
