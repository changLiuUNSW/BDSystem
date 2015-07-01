using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DataAccess.EntityFramework;
using DataAccess.EntityFramework.Models.BD.Lead;
using DateAccess.Services.ContactService.Call.Exceptions;
using DateAccess.Services.ContactService.Call.Scripts.Actions;
using DateAccess.Services.MailService;

namespace DateAccess.Services.ContactService.Call.Providers
{
    /// <summary>
    /// script provider factory
    /// </summary>
    public abstract class CallProvider
    {
        protected readonly IUnitOfWork UnitOfWork;
        public ILeadEmailService EmailService { get; set; }

        protected CallProvider(IUnitOfWork unitOfWork,
                        ILeadEmailService emailService)
        {
            UnitOfWork = unitOfWork;
            EmailService = emailService;
        }

        protected virtual int SaveOccupied(int contactId, int telesaleId, int? personId = null)
        {
            var occupied = new OccupiedContact
            {
                ContactId = contactId,
                LeadPersonalId = personId,
                TelesaleId = telesaleId,
                TimeStarted = DateTime.Now
            };

            UnitOfWork.OccupiedContactRepository.Add(occupied);
            UnitOfWork.Save();

            return occupied.Id;
        }

        public IStandardCall StandardCall
        {
            get
            {
                var call = this as IStandardCall;

                if (call == null)
                    throw new Exception("Unable to cast to standard call");

                return call;
            }
        }

        public IGroupCall GroupCall
        {
            get
            {
                var call = this as IGroupCall;

                if (call == null)
                    throw new Exception("Unable to cast  to group call");

                return call;
            }
        }

        public virtual void EndCall(int contactId, int leadPersonId, int occupiedId, string initial, string url, IList<ScriptAction> actions)
        {
            var contact = UnitOfWork.ContactRepository.Get(contactId);
            if (contact == null)
                throw new InvalidContactIdException(contactId);

            var leadPerson = UnitOfWork.LeadPersonalRepository.Get(leadPersonId);

            //in the case there is no lead person, set the lead person to DHUD
            if (leadPerson == null)
                leadPerson = UnitOfWork.LeadPersonalRepository.SingleOrDefault(x => x.Initial == "DHUD");

            var telesale = UnitOfWork.TelesaleRepository.SingleOrDefault(x => x.Initial == initial);
            var leadsBefore = contact.Leads.Count;
            foreach (var action in actions)
            {
                if (action == null)
                    continue;

                action.Update(contact, leadPerson, telesale);
            }

            UnitOfWork.OccupiedContactRepository.Delete(occupiedId);
            UnitOfWork.Save();

            var cancelToken = new CancellationTokenSource();

            //if there is a new lead, send a emails
            //spawn a new thread to handle email so errors will not block the main threads
            if (leadsBefore < contact.Leads.Count)
            {
                var task = Task.Run(() =>
                {
                    try
                    {
                        var lead = contact.Leads.Last();
                        var leadUrl = url == null ? null : url.Replace("0", lead.Id.ToString(CultureInfo.InvariantCulture));
                        EmailService.SendNewLeadEmail(lead, leadUrl);
                    }
                    catch (Exception)
                    {
                        cancelToken.Cancel();
                    }
                }, cancelToken.Token);

                //must wait otherwise there is a chance that data context will be disposed before task completion
                task.Wait(cancelToken.Token);
            }
        }
    }
}
