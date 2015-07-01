using DataAccess.EntityFramework.Models.BD.Contact;
using DataAccess.EntityFramework.Models.BD.Lead;
using DataAccess.EntityFramework.Models.BD.Telesale;
using DataAccess.EntityFramework.TypeLibrary;
using DateAccess.Services.ContactService.Leads;

namespace DateAccess.Services.ContactService.Call.Scripts.Actions
{
    public class CreateTaskForDh : ScriptAction
    {
        public CreateTaskForDh() : base("Create task for DH.", ScriptActionType.TaskForDH)
        {
            LeadAssembler = new LeadAssembler
            {
                LeadFactory = new LeadFactory()
            };
        }

        private LeadAssembler LeadAssembler { get; set; }

        public override void Update(Contact contact, LeadPersonal person, Telesale telesale)
        {
            LeadAssembler.Contact = contact;
            LeadAssembler.LeadPersonal = person;
            LeadAssembler.Telesale = telesale;
            contact.Leads.Add(LeadAssembler.Assemble(BusinessTypes.Cleaning)); 
        }
    }
}
