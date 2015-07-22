using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DataAccess.EntityFramework;
using DataAccess.EntityFramework.Models.BD.Contact;
using DataAccess.EntityFramework.Models.BD.Lead;
using DataAccess.EntityFramework.Models.BD.Site;
using DataAccess.EntityFramework.Models.BD.Telesale;
using DataAccess.EntityFramework.TypeLibrary;
using DateAccess.Services.ContactService.Call.Exceptions;
using DateAccess.Services.ContactService.Call.Models;
using DateAccess.Services.ContactService.Call.Queues;
using DateAccess.Services.ContactService.Call.Scripts.Actions;
using DateAccess.Services.ContactService.Call.Scripts.Providers;
using DateAccess.Services.MailService;

namespace DateAccess.Services.ContactService.Call.Providers
{
    internal class TelesaleCallProvider : CallProvider, IStandardCall, IGroupCall
    {
        internal enum CallingCode
        {
            OPR
        }

        private IList<ScriptAction> DefaultActions { get; set; }

        internal TelesaleCallProvider(IUnitOfWork unitOfWork,
            IEmailHelper emailHelper)
            : base(unitOfWork, emailHelper)
        {
            DefaultActions = InitDefaultActions();
        }


        internal TelesaleCallProvider(IUnitOfWork unitOfWork,
            Telesale telesale,
            IEmailHelper emailHelper)
            : base(unitOfWork, emailHelper)
        {
            Telesale = telesale;
            DefaultActions = InitDefaultActions();
        }

        private IList<ScriptAction> InitDefaultActions()
        {
            return new Collection<ScriptAction>
            {
                new UpdateDaCheck(),
                new UpdateExtManager(),
                new UpdateSendInfo(),
                new UpdateCallBack()
            };
        }

        private readonly static object Locker = new object();

        public Telesale Telesale { get; set; }
        public CleaningQueue CleaningQueue { get; set; }

        protected CallDetail Fetch()
        {
            var callDetail = FetchAssignment();

            if (callDetail != null) 
                return callDetail;

            if (CleaningQueue != null) 
                return FetchOpr();

            var config = PriorityConfig.GetConfig();
            var priorities = UnitOfWork.PriorityGroupRepository.Get();
            var leadPersons = UnitOfWork.LeadPersonalRepository.Include(x => x.LeadGroup, x => x.Leads);
            CleaningQueue = new CleaningQueue(leadPersons, priorities, config);
            return FetchOpr();
        }

        protected bool Prepare(string initial)
        {
            if (Telesale == null && string.IsNullOrEmpty(initial))
                throw new Exception("Initial required!");

            if (Telesale == null)
            {
                Telesale =
                    UnitOfWork.TelesaleRepository.Filter(x => x.Initial == initial, x => x, x => x.Assignments)
                        .SingleOrDefault();
            }

            if (Telesale == null)
                throw new Exception("Invalid initial!");

            var occupiedCall = UnitOfWork.OccupiedContactRepository.SingleOrDefault(x => x.TelesaleId == Telesale.Id);
            if (occupiedCall != null)
            {
                var callDetail = new CallDetail
                {
                    OccupiedId = occupiedCall.Id,
                    LeadPerson = occupiedCall.LeadPersonal,
                    Contact = occupiedCall.Contact,
                    Site = occupiedCall.Contact.Site
                };


                callDetail.Script = new TelesaleScriptProvider(occupiedCall.Contact, occupiedCall.Telesale,
                    occupiedCall.LeadPersonal).Get();

                throw new UnfinishedCallException(
                    "Same initial is been used by another caller or the last call has not been successfuly confirmed.",
                    callDetail);
            }

            return true;
        }

        public override void EndCall(
            int siteId, int? contactId, int? leadPersonId, int? occupiedId, string initial, string url, IList<ScriptAction> actions)
        {
            if (string.IsNullOrEmpty(initial))
                throw new Exception("Unable to register a call line due to invalid initial");

            var contact = UnitOfWork.ContactRepository.Get(contactId);
            if (contact != null)
            {
                var today = DateTime.Now;
                var callLine = new CallLine
                {
                    Initial = initial,
                    TimeCreated = today,
                    LastContact = today
                };

                contact.CallLines.Add(callLine);
            }

            base.EndCall(siteId, contactId, leadPersonId, occupiedId, initial, url, actions);
        }

        private CallDetail FetchAssignment()
        {
            var contact = UnitOfWork.ContactRepository.NextCleaningContact(Telesale.Id);

            CallDetail detail = null;
            if (contact != null)
            {
                detail = new CallDetail
                {
                    OccupiedId = SaveOccupied(contact.Id, Telesale.Id),
                    Contact = contact,
                    Site = contact.Site,
                    ScriptActions = DefaultActions
                };

                detail.Script = new TelesaleScriptProvider(contact, Telesale, null).Get();
            }

            return detail;
        }

        private CallDetail FetchOpr()
        {
            if (CleaningQueue == null)
                throw new Exception("Invalid call queue");

            lock (Locker)
            {
                foreach (var queue in CleaningQueue.GetQueue())
                {
                    var person = queue.Key;
                    if (person == null)
                        return null;

                    var contact = NextContact(person);
                    if (contact == null)
                        continue;

                    var callDetail = new CallDetail
                    {
                        OccupiedId = SaveOccupied(contact.Id, Telesale.Id, person.Id),
                        Contact = contact,
                        LeadPerson = person,
                        Site = contact.Site,
                        ScriptActions = DefaultActions
                    };

                    callDetail.Script = new TelesaleScriptProvider(contact, Telesale, person).Get();
                    return callDetail;
                }

                return null;
            }
        }

        private Contact NextContact(LeadPersonal person)
        {

            IList<int> occupiedList;
            if (OverOccupyLimit(person, out occupiedList))
                return null;

            var codes = Enum.GetNames(typeof(CallingCode));
            return occupiedList.Count > 0
                ? UnitOfWork.ContactRepository.NextCleaningContact(codes, person.Id, occupiedList)
                : UnitOfWork.ContactRepository.NextCleaningContact(codes, person.Id);
        }

        private bool OverOccupyLimit(LeadPersonal leadPerson, out IList<int> occupiedList)
        {
            occupiedList = UnitOfWork.OccupiedContactRepository.Filter(x => x.LeadPersonalId == leadPerson.Id, x => x.ContactId);
            if (occupiedList.Any() && occupiedList.Count >= leadPerson.GetLeadsLeftToGet())
                return true;

            return false;
        }

        public CallDetail Next(string initial)
        {
            return Prepare(initial) ? Fetch() : null;
        }

        public CallDetail Next(string name, int siteId)
        {
            var site = UnitOfWork.SiteRepository.Get(siteId);
            if (site == null)
                return null;

            var contact = site.Contacts.SingleOrDefault(x => x.BusinessTypeId == (int) BusinessTypes.Cleaning);
            return new CallDetail
            {
                Contact = contact,
                Site = site,
                Script = new TelesaleScriptProvider(contact, Telesale, null).Get(),
                ScriptActions = DefaultActions
            };
        }
    }
}
