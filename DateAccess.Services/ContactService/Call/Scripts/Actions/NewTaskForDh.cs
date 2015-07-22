using DataAccess.EntityFramework.Models.BD.Contact;
using DataAccess.EntityFramework.Models.BD.Lead;
using DataAccess.EntityFramework.Models.BD.Site;
using DataAccess.EntityFramework.Models.BD.Telesale;
using DataAccess.EntityFramework.TypeLibrary;
using DateAccess.Services.ContactService.Leads;

namespace DateAccess.Services.ContactService.Call.Scripts.Actions
{
    public class NewTaskForDh : ScriptAction
    {
        public NewTaskForDh() : base("New task for DH.", ScriptActionType.NewTaskForDH)
        {
            LeadAssembler = new LeadAssembler
            {
                LeadFactory = new LeadFactory()
            };
        }

        private LeadAssembler LeadAssembler { get; set; }

        public override ScriptActionResult Update(Site site, Contact contact, LeadPersonal person, Telesale telesale)
        {
            LeadAssembler.Contact = contact;
            LeadAssembler.LeadPersonal = person;
            LeadAssembler.Telesale = telesale;
            contact.Leads.Add(LeadAssembler.Assemble(BusinessTypes.Cleaning));
            return ScriptActionResult.Completed | ScriptActionResult.EmailRequired;
        }
    }
}
