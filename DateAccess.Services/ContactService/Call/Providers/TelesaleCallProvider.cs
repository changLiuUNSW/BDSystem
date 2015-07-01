using System;
using System.Collections.Generic;
using System.Linq;
using DataAccess.EntityFramework;
using DataAccess.EntityFramework.Models.BD.Contact;
using DataAccess.EntityFramework.Models.BD.Lead;
using DataAccess.EntityFramework.Models.BD.Site;
using DataAccess.EntityFramework.Models.BD.Telesale;
using DateAccess.Services.ContactService.Call.Exceptions;
using DateAccess.Services.ContactService.Call.Models;
using DateAccess.Services.ContactService.Call.Queues;
using DateAccess.Services.ContactService.Call.Scripts.Actions;
using DateAccess.Services.ContactService.Call.Scripts.Providers;
using DateAccess.Services.MailService;

namespace DateAccess.Services.ContactService.Call.Providers
{
    internal class TelesaleCallProvider : CallProvider, IStandardCall
    {
        internal enum CallingCode
        {
            OPR
        }

        internal TelesaleCallProvider(IUnitOfWork unitOfWork,
            ILeadEmailService emailService)
            : base(unitOfWork, emailService) { }


        internal TelesaleCallProvider(IUnitOfWork unitOfWork,
            Telesale telesale,
            ILeadEmailService emailService) : base(unitOfWork, emailService)
        {
            Telesale = telesale;
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
                throw new Exception("Invalid telesale!");

            if (Telesale == null)
            {
                Telesale =
                    UnitOfWork.TelesaleRepository.Filter(x => x.Initial == initial, x => x, x => x.Assignments)
                        .SingleOrDefault();
            }

            if (Telesale == null)
                throw new Exception("Invalid telesale!");

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


                callDetail.Script = new ScriptProvider(occupiedCall.Contact, occupiedCall.Telesale,
                    occupiedCall.LeadPersonal).Get();

                throw new UnfinishedCallException(
                    "Same initial is been used by another caller or the last call has not been successfuly confirmed.",
                    callDetail);
            }

            return true;
        }

        public override void EndCall(int contactId, int leadPersonId, int occupiedId, string initial, string url, IList<ScriptAction> actions)
        {
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

            base.EndCall(contactId, leadPersonId, occupiedId, initial, url, actions);
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
                    Site = contact.Site
                };

                detail.Script = new ScriptProvider(contact, Telesale, null).Get();
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
                        Site = contact.Site
                    };

                    callDetail.Script = new ScriptProvider(contact, Telesale, person).Get();
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
    }
}
