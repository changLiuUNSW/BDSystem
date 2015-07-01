using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DataAccess.EntityFramework;
using DataAccess.EntityFramework.Models.BD.Contact;
using DateAccess.Services.ViewModels;

namespace DateAccess.Services.ContactService
{
    public class UpdatePersonModel
    {
        //contactId
        public int Id;
        public int? ContactPersonId;

    }

    public interface IContactPersonService :IRepositoryService<ContactPerson>
    {
        int UpdatePersonForContact(IList<UpdatePersonModel> updates);
        IList<ContactPersonHistory> GetChangeHistory(string reason = null, int? personId = null);
        IList<ContactPersonHistory> GetNameCorrectHistoryList();
        int UpdatePendingChanges(IList<PendingChange> changes);
        new int Delete<TKey>(TKey key);
        void RemovePersonHistories(int[] ids);
        void HistoryChangeToNewManager(List<int> ids);
    }

    internal class ContactPersonService : RepositoryService<ContactPerson>, IContactPersonService
    {
        public ContactPersonService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public int UpdatePersonForContact(IList<UpdatePersonModel> updates)
        {
            var contactIds = updates.Select(x => x.Id);
            var contacts = UnitOfWork.ContactRepository.Get(x=>contactIds.Contains(x.Id));

            foreach (var update in updates)
            {
                var contact = contacts.Single(x => x.Id == update.Id);
                contact.ContactPersonId = update.ContactPersonId;
            }

            return Save();
        }

        public IList<ContactPersonHistory> GetChangeHistory(string reason = null, int? personId = null)
        {
            var history =
                UnitOfWork.ContactPersonHistoryRepository.Get(x => x.OriginalContactPersonId == personId)
                    .OrderByDescending(x => x.Time).ToList();

            if (!string.IsNullOrEmpty(reason))
                return history.Where(x => x.ReasonForChange == reason.ToLower()).ToList();

            return history;
        }


        public IList<ContactPersonHistory> GetNameCorrectHistoryList()
        {
            return UnitOfWork.ContactPersonHistoryRepository.Get(l=>l.ReasonForChange=="Name Correction");
        }

        public int UpdatePendingChanges(IList<PendingChange> changes)
        {
            foreach (var change in changes)
            {
                UnitOfWork.ContactPersonRepository.Update(change.Update);
                var history = Mapper.Map<ContactPersonHistory>(change.History);
                history.EditName = change.Name;
                history.ReasonForChange = change.Reason;
                history.Time = DateTime.Now;
                history.OriginalContactPersonId = change.Update.Id;
                UnitOfWork.ContactPersonHistoryRepository.Add(history);
            }

            return UnitOfWork.Save();
        }


        public ContactPerson AddHistory(ContactPerson contactPerson)
        {
            var person = GetByKey(contactPerson.Id);
            var history = Mapper.Map<ContactPersonHistory>(person);
            history.Time = DateTime.Now;
            history.OriginalContactPersonId = person.Id;
            UnitOfWork.ContactPersonHistoryRepository.Add(history);
            UnitOfWork.Save();
            return contactPerson;
        }

        public override int Delete<TKey>(TKey key)
        {
            var person = GetByKey(key);
            
            if (person == null)
                throw new Exception("Person not found in the database!");

            foreach (var contact in person.Contacts)
            {
                contact.ContactPersonId = null;
            }
             
            return base.Delete(key);
        }

        public void RemovePersonHistories(int [] ids)
        {
            var historyRepo = UnitOfWork.ContactPersonHistoryRepository;
            UnitOfWork.ContactPersonHistoryRepository.RemoveRange(historyRepo.Get(l => ids.Contains(l.Id)));
            Save();
        }

        public void HistoryChangeToNewManager(List<int> ids)
        {
            var historyRepo = UnitOfWork.ContactPersonHistoryRepository;
            var historyList = historyRepo.Get(l => ids.Contains(l.Id)).OrderBy(l=>l.Time).ToList();
            foreach (var history in historyList)
            {
                var contacts= history.ContactPerson.Contacts.ToList();
                foreach (var contact in contacts)
                {
                    contact.NewManagerDate = history.Time;
                }
            }
            UnitOfWork.ContactPersonHistoryRepository.RemoveRange(historyList);
            Save();
        }
    }
}
