using System;
using System.Linq;
using DataAccess.EntityFramework;
using DataAccess.EntityFramework.Models.BD.Lead;
using DataAccess.EntityFramework.TypeLibrary;
using DateAccess.Services.ContactService.Call.Models;
using DateAccess.Services.ContactService.Call.Scripts.Providers;
using DateAccess.Services.MailService;

namespace DateAccess.Services.ContactService.Call.Providers
{
    internal class BdCallProvider : CallProvider, IStandardCall, IGroupCall
    {
        internal BdCallProvider(IUnitOfWork unitOfWork, 
                                ILeadEmailService emailService
                                ) : base(unitOfWork, emailService)
        {
        }

        public LeadPersonal LeadPerson { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="initial">initial is the login name</param>
        /// <returns></returns>
        protected bool Prepare(string initial)
        {
            if (LeadPerson == null && string.IsNullOrEmpty(initial))
                throw new Exception("Invalid lead person");

            if (LeadPerson == null)
            {
                LeadPerson = UnitOfWork.LeadPersonalRepository.GetFromPhoneBook(initial);
                if (LeadPerson == null)
                    throw new Exception("User not found in the lead person list");
            }

            return true;
        }

        protected CallDetail Fetch()
        {
            var code = LeadPerson.Initial.Substring(0, 2);
            var contact = UnitOfWork.ContactRepository.NextCleaningContact(new[] {code}, false);

            if (contact == null)
                return null;

            return new CallDetail
            {
                Contact = contact,
                Site = contact.Site,
                LeadPerson = LeadPerson
            };
        }

        public CallDetail Next(string initial)
        {
            return Prepare(initial) ? Fetch() : null;
        }

        public CallDetail Next(int siteId)
        {
            var site = UnitOfWork.SiteRepository.Get(siteId);
            if (site == null)
                return null;

            var contact = site.Contacts.SingleOrDefault(x => x.BusinessTypeId == (int)BusinessTypes.Cleaning);

            return new CallDetail
            {
                Contact = contact,
                Site = site,
                Script = new ScriptProvider(contact, null, null).Get()
            };
        }
    }
}
