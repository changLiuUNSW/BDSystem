using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DataAccess.EntityFramework;
using DataAccess.EntityFramework.Models.BD.Contact;
using DataAccess.EntityFramework.Models.BD.Lead;
using DataAccess.EntityFramework.Models.BD.Site;
using DataAccess.EntityFramework.Models.BD.Telesale;
using DataAccess.EntityFramework.TypeLibrary;
using DateAccess.Services.ContactService.Call.Models;
using DateAccess.Services.ContactService.Call.Scripts.Actions;
using DateAccess.Services.MailService;
using NewLead = DateAccess.Services.Events.NewLead;

namespace DateAccess.Services.ContactService.Call.Providers
{
    /// <summary>
    /// script provider factory
    /// </summary>
    public abstract class CallProvider
    {
        protected readonly IUnitOfWork UnitOfWork;
        protected readonly IEmailHelper EmailHelper;


        protected CallProvider(IUnitOfWork unitOfWork,IEmailHelper emailHelper)
        {
            UnitOfWork = unitOfWork;
            EmailHelper = emailHelper;
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

        public static CallProvider Create(CallType type, IUnitOfWork unitOfWork, IEmailHelper emailHelper)
        {
            switch (type)
            {
                case CallType.Telesale:
                    return new TelesaleCallProvider(unitOfWork, emailHelper);
                case CallType.BD:
                    return new BdCallProvider(unitOfWork, emailHelper);
                default:
                    throw new Exception("Incorrect call type");
            }
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

        public virtual void EndCall(
            int siteId, int? contactId, int? leadPersonId, int? occupiedId, string initial, string url, IList<ScriptAction> actions)
        {
            var site = UnitOfWork.SiteRepository.Get(siteId);

            if (site == null)
                throw new Exception("Invalid site");

            Contact contact = null;
            LeadPersonal leadPerson = null;

            if (contactId.HasValue)
                contact = UnitOfWork.ContactRepository.Get(contactId);

            if (leadPersonId.HasValue)
                leadPerson = UnitOfWork.LeadPersonalRepository.Get(leadPersonId);

            var telesale = UnitOfWork.TelesaleRepository.SingleOrDefault(x => x.Initial == initial);
            var actionResult = ScriptActionResult.Completed;
            for (int i = 0; i < actions.Count; i++)
            {
                var action = actions[i];
                var result = action.Update(site, contact, leadPerson, telesale);

                if (i != 0)
                    actionResult = actionResult | result;
                else
                    actionResult = result;
            }

            UnitOfWork.OccupiedContactRepository.Delete(occupiedId);
            UnitOfWork.Save();

            var cancelToken = new CancellationTokenSource();

            //if there is a new lead, send a emails
            //spawn a new thread to handle email so errors will not block the main threads
            if (actionResult.HasFlag(ScriptActionResult.EmailRequired))
            {
                var task = Task.Run(() =>
                {
                    try
                    {
                        var lead = contact.Leads.Last();
                        var leadUrl = url == null ? null : url.Replace("0", lead.Id.ToString(CultureInfo.InvariantCulture));
                        var newLead = Mapper.Map<NewLead>(lead);
                        newLead.Url = leadUrl;
                        newLead.Email = EmailHelper.GetEmailByInitial(lead.LeadPersonal.Initial);
                        DomainEvents.Raise(newLead);
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
