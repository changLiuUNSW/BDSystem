using System;
using System.Linq;
using DataAccess.EntityFramework.Models.BD.Contact;
using DataAccess.EntityFramework.Models.BD.Lead;
using DataAccess.EntityFramework.Models.BD.Telesale;
using DataAccess.EntityFramework.TypeLibrary;

namespace DateAccess.Services.ContactService.Leads
{
    /// <summary>
    /// assemble lead information from contact / lead personal and telelsales
    /// </summary>
    internal class LeadAssembler
    {
        public LeadAssembler()
        {
            
        }

        public LeadAssembler(Contact contact, LeadPersonal leadPersonal, Telesale telesale, LeadFactory factory)
        {
            Contact = contact;
            LeadPersonal = leadPersonal;
            Telesale = telesale;
            LeadFactory = factory;
        }

        public Contact Contact { get; set; }
        public LeadFactory LeadFactory { get; set; }
        public LeadPersonal LeadPersonal { get; set; }
        public Telesale Telesale { get; set; }

        public virtual Lead Assemble(BusinessTypes type)
        {
            if (LeadFactory == null)
                throw new Exception("Internal server error, contact admin for more information");

            if (Contact == null)
                throw new Exception("Unable to generate lead from an empty contact");

            if (LeadPersonal == null)
                throw new Exception("Unable to generate lead because no QP has been assigned to it");

            if (Contact.Leads.Any(x=>x.LeadStatusId != (int) LeadStatusTypes.Cancelled))
                throw new Exception("Contact already has a lead");


            LeadFactory.Contact = Contact;
            var lead = LeadFactory.GetLead(type);
            lead.LeadPersonalId = LeadPersonal.Id;

            if (Telesale != null)
                lead.TelesaleId = Telesale.Id;

            return lead;
        }
    }
}
