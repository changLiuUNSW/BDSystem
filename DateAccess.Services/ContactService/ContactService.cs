using System;
using System.Collections.Generic;
using DataAccess.EntityFramework;
using DataAccess.EntityFramework.Models.BD.Contact;
using DataAccess.EntityFramework.Models.BD.Lead;
using DataAccess.EntityFramework.Models.BD.Site;
using DataAccess.EntityFramework.TypeLibrary;
using DateAccess.Services.ContactService.Call.Models;
using DateAccess.Services.ContactService.Call.Queues;
using DateAccess.Services.ContactService.Reports;

namespace DateAccess.Services.ContactService
{
    public interface IContactService : IRepositoryService<Contact>
    {
        ReportProvider ReportProvider { get; }
        IQueue<IDictionary<LeadPersonal, string>> Priority { get; }
        int UpdateNote(int contactId, string note);
        int DeleteCallLine(int contactId, int id);
        CallLine AddCallLine(int contactId, CallLine callLine);
        Contact AssignContactperson(int contactId, int contactPersonId, int targetContactId, BusinessTypes type);
        void RemoveOccupiedCall(int id);
    }


    internal class ContactService : RepositoryService<Contact>, IContactService
    {
        private ReportProvider _reportProvider;

        public ContactService(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            var persons = UnitOfWork.LeadPersonalRepository.Include(x=>x.LeadGroup, x=>x.Leads);
            var priorities = UnitOfWork.PriorityGroupRepository.Get();
            var config = PriorityConfig.GetConfig();
            Priority = new CleaningQueue(persons, priorities, config);
        }

        public ReportProvider ReportProvider
        {
            get
            {
                if (_reportProvider == null)
                    _reportProvider = new ReportProvider(UnitOfWork);

                return _reportProvider;
            }
        }


        public IQueue<IDictionary<LeadPersonal, string>> Priority { get; private set; }

        public int UpdateNote(int contactId, string note)
        {
            UnitOfWork.ContactRepository.Get(contactId).Note = note;
            return Save();
        }

        /// <summary>
        /// remove a call line from the target contact 
        /// </summary>
        /// <param name="contactId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeleteCallLine(int contactId, int id)
        {
            UnitOfWork.CallLineRepository.Delete(id);
            return UnitOfWork.Save();
        }

        /// <summary>
        /// add a new call line into the target contact
        /// </summary>
        /// <param name="contactId"></param>
        /// <param name="callLine"></param>
        /// <returns></returns>
        public CallLine AddCallLine(int contactId, CallLine callLine)
        {
            var contact = UnitOfWork.ContactRepository.Get(contactId);

            if (contact == null)
                return null;

            callLine.LastContact = DateTime.Today;
            callLine.TimeCreated = DateTime.Today;

            contact.CallLines.Add(callLine);
            UnitOfWork.Save();
            return callLine;
        }

        /// <summary>
        /// assignment the contact person to the target contact
        /// </summary>
        /// <returns></returns>
        public Contact AssignContactperson(int contactId, int contactPersonId, int targetContactId, BusinessTypes type)
        {
            var contactPerson = UnitOfWork.ContactPersonRepository.Get(contactPersonId);

            if (contactPerson == null)
                throw new Exception(string.Format("Contact person for id {0} was not found in the system.", contactPersonId));

            Contact targetContact = null;

            if (targetContactId > 0)
            {
                targetContact = UnitOfWork.ContactRepository.Get(targetContactId);
                if (targetContact == null)
                    throw new Exception(string.Format("Target contact for id {0} was not found in the system.", targetContactId));
            }
            else
            {
                var contact = UnitOfWork.ContactRepository.Get(contactId);
                if (contact == null)
                    throw new Exception(string.Format("Contact for id {0} was not found in the system.", contactId));

                targetContact = new Contact
                {
                    CallFrequency = contact.CallFrequency,
                    SiteId = contact.SiteId,
                    NextCall = DateTime.Today,
                    Code = contact.Code,
                    BusinessTypeId = (int)type
                };

                UnitOfWork.ContactRepository.Add(targetContact);
            }

            AssignContactperson(targetContact, contactPerson);
            UnitOfWork.Save();
            return targetContact;
        }

        private void AssignContactperson(Contact contact, ContactPerson person)
        {
            if (contact.ContactPersonId == person.Id)
            {
                contact.ContactPersonId = null;
            }
            else
            {
                contact.ContactPersonId = person.Id;
                if (person.CreateDate == DateTime.Today)
                    contact.NewManagerDate = DateTime.Today;
            }
        }

        public void RemoveOccupiedCall(int id)
        {
            UnitOfWork.OccupiedContactRepository.Delete(id);
            UnitOfWork.Save();
        }
    }
}
