using System;
using DataAccess.EntityFramework.Models.BD;
using DataAccess.EntityFramework.Models.BD.Contact;
using DataAccess.EntityFramework.Models.BD.Lead;
using DataAccess.EntityFramework.TypeLibrary;

namespace DateAccess.Services.ContactService.Leads
{
    /// <summary>
    /// base lead factory for create new lead from the contact
    /// </summary>
    internal class LeadFactory
    {
        public LeadFactory()
        {
            
        }

        public LeadFactory(Contact contact)
        {
            Contact = contact;
        }

        public Contact Contact { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }

        public virtual Lead GetLead(BusinessTypes type)
        {
            if (Contact == null)
                throw new Exception("Can not generate lead from an empty contact");

            if (Contact.Site == null)
                throw new Exception("Can not generate lead without a valid site");

            var phone = Phone ?? GetPhoneNumber(Contact);

            if (phone == null)
                throw new Exception("Invalid phone number");

            var lead = new Lead
            {
                Address = new Address
                {
                    Number = Contact.Site.Number,
                    Street = Contact.Site.Street,
                    Suburb = Contact.Site.Suburb,
                    Unit = Contact.Site.Unit
                },

                ContactId = Contact.Id,
                Phone = phone,
                Postcode = Contact.Site.Postcode,
                State = Contact.Site.State,
                BusinessTypeId = (int)type,
                LeadStatusId = (int)LeadStatusTypes.New
            };

            var time = DateTime.Now;
            lead.CreatedDate = time;
            lead.LastUpdateDate = time;
            return lead;
        }

        protected virtual string GetPhoneNumber(Contact contact)
        {
            if (contact == null)
                return null;

            if (contact.ContactPerson != null)
            {
                if (!string.IsNullOrEmpty(contact.ContactPerson.DirectLine))
                    return contact.ContactPerson.DirectLine;

                if (!string.IsNullOrEmpty(contact.ContactPerson.Mobile))
                    return contact.ContactPerson.Mobile;
            }

            if (contact.Site != null)
            {
                if (!string.IsNullOrEmpty(contact.Site.Phone))
                    return contact.Site.Phone;
            }

            return null;
        }
    }
}
