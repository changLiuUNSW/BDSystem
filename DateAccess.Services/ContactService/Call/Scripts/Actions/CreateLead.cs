using System;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using DataAccess.EntityFramework.Models.BD;
using DataAccess.EntityFramework.Models.BD.Contact;
using DataAccess.EntityFramework.Models.BD.Lead;
using DataAccess.EntityFramework.Models.BD.Telesale;
using DataAccess.EntityFramework.TypeLibrary;
using DateAccess.Services.ContactService.Leads;

namespace DateAccess.Services.ContactService.Call.Scripts.Actions
{
    public class CreateLead : ScriptAction
    {
        public CreateLead() { Init(); }
        public CreateLead(string msg, ScriptActionType type) : base(msg, type) { Init(); }

        public string Phone { get; set; }
        public string Address { get; set; }

        private LeadAssembler LeadAssembler { get; set; }

        private void Init()
        {
            Phone = null;
            Address = null;
            LeadAssembler = new LeadAssembler {LeadFactory = new LeadFactory()};
        }

        public override void Update(Contact contact, LeadPersonal person, Telesale telesale)
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
        }
    }
}
