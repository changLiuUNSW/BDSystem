using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DataAccess.EntityFramework;
using DataAccess.EntityFramework.Models.BD.Lead;
using DataAccess.EntityFramework.TypeLibrary;
using DateAccess.Services.ContactService.Call.Models;
using DateAccess.Services.ContactService.Call.Scripts.Actions;
using DateAccess.Services.ContactService.Call.Scripts.Providers;
using DateAccess.Services.MailService;

namespace DateAccess.Services.ContactService.Call.Providers
{
    internal class BdCallProvider : CallProvider, IStandardCall, IGroupCall
    {
        private IList<ScriptAction> DefaultActions { get; set; } 

        internal BdCallProvider(IUnitOfWork unitOfWork, 
                                IEmailHelper emailHelper
                                ) : base(unitOfWork, emailHelper)
        {
            DefaultActions = new Collection<ScriptAction>
            {
                new UpdateDaCheck(),
                new NewLead(),
                new UpdateCallBack()
            };
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
                LeadPerson = LeadPerson,
                Script = new BdScriptProvider(contact, LeadPerson ).Get(),
                ScriptActions = DefaultActions
            };
        }

        public CallDetail Next(string loginName)
        {
            return Prepare(loginName) ? Fetch() : null;
        }

        public CallDetail Next(string loginName, int siteId)
        {
            var site = UnitOfWork.SiteRepository.Get(siteId);
            if (site == null)
                return null;

            var contact = site.Contacts.SingleOrDefault(x => x.BusinessTypeId == (int)BusinessTypes.Cleaning);
            LeadPerson = UnitOfWork.LeadPersonalRepository.GetFromPhoneBook(loginName);

            return new CallDetail
            {
                Contact = contact,
                Site = site,
                LeadPerson = LeadPerson,
                Script = new BdScriptProvider(contact, LeadPerson).Get(),
                ScriptActions = DefaultActions
            };
        }
    }
}
