using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DataAccess.Common;
using DataAccess.EntityFramework;
using DataAccess.EntityFramework.Models.BD;
using DataAccess.EntityFramework.Models.BD.Contact;
using DataAccess.EntityFramework.Models.BD.Lead;
using DataAccess.EntityFramework.Models.BD.Telesale;
using DataAccess.EntityFramework.Models.Quote;
using DataAccess.EntityFramework.TypeLibrary;
using DateAccess.Services.ContactService.Leads;
using DateAccess.Services.Events;
using DateAccess.Services.MailService;

namespace DateAccess.Services.ContactService
{
    public class AppointmentModel
    {
        public DateTime? AppointmentDate { get; set; }
        public string Url { get; set; }
    }

    public class UpdateQpModel
    {
        public int QpId { get; set; }
        public string Url { get; set; }
    }

    public interface ILeadService:IRepositoryService<Lead>
    {
        Task<List<LeadStatus>> GetAllLeadStatus();
        Task<Lead> UpdateAppointment(int id, string user, AppointmentModel appointmentModel);
        Task<Lead> UpdateCallback(int id, string user, DateTime? callbackDate);
        Lead GenerateQuote(int id, string user);
        Task<Lead> ContactNotSuccess(int id, string user);
        Task<Lead> Visited(int id, string user);
        Task<Lead> Cancel(int id, string user);
        Task<Lead> UpdateQp(int id, string user, UpdateQpModel updateQpModel);
        Task<List<LeadPersonal>> GetAllQpByZone(string zone);
        Lead NewLead(int contactId, int? leadPersonId, int? telesaleId);
    }

    internal class LeadService :RepositoryService<Lead>, ILeadService
    {
        private readonly ApplicationSettings _settings;
        private readonly IEmailHelper _emailHelper;
        public LeadService(IUnitOfWork unitOfWork, ApplicationSettings settings,IEmailHelper emailHelper)
            : base(unitOfWork)
        {
            _settings = settings;
            _emailHelper = emailHelper;
        }

        public async Task<List<LeadStatus>> GetAllLeadStatus()
        {
            var statusList = await UnitOfWork.GetRepository<LeadStatus>().GetAsync();
            return statusList;
        }

        public async Task<Lead> UpdateAppointment(int id, string user, AppointmentModel appointmentModel)
        {
            const int status = (int) LeadStatusTypes.Appointment;
            var dateDescrption = appointmentModel.AppointmentDate.HasValue
                ? appointmentModel.AppointmentDate.Value.ToString("dd/MM/yyyy")
                : "Null";
            var description = "update appointment to " + dateDescrption;
            var lead = UpdateStatusAndHistory(id, status, user, description);
            lead.AppointmentDate = appointmentModel.AppointmentDate;
            await UnitOfWork.SaveAsync();
            var leadAppointment = Mapper.Map<LeadAppointment>(lead);
            leadAppointment.Url = appointmentModel.Url;
            leadAppointment.Email = _emailHelper.GetEmailByInitial(lead.LeadPersonal.Initial);
            await DomainEvents.Raise(leadAppointment);
            return lead;
        }

        public async Task<Lead> UpdateCallback(int id, string user, DateTime? callbackDate)
        {
            const int status = (int) LeadStatusTypes.CallBack;
            var dateDescrption = callbackDate.HasValue ? callbackDate.Value.ToString("dd/MM/yyyy") : "Null";
            var description = "update callback to " + dateDescrption;
            var lead = UpdateStatusAndHistory(id, status, user, description);
            lead.CallBackDate = callbackDate;
            await UnitOfWork.SaveAsync();
            return lead;
        }


        public Lead GenerateQuote(int id, string user)
        {
            const int status = (int) LeadStatusTypes.Estimation;
            const string description = "new quote created";
            var lead = UpdateStatusAndHistory(id, status, user, description);
            if (UnitOfWork.QuoteRepository.Any(l => l.LeadId == lead.Id)) 
                throw new Exception("The quote for this lead already exists, please contact IT department");
            var quote = CreateQuote(lead);
            UnitOfWork.QuoteRepository.Add(quote);
            UnitOfWork.Save();
            return lead;
        }

        public async Task<Lead> ContactNotSuccess(int id, string user)
        {
            const int status = (int) LeadStatusTypes.ToBeCalled;
            const string description = "contact not successfully";
            var lead = UpdateStatusAndHistory(id, status, user, description);
            await UnitOfWork.SaveAsync();
            return lead;
        }

        public async Task<Lead> Visited(int id, string user)
        {
            const int status = (int) LeadStatusTypes.Visited;
            const string description = "visited but cannot make a quote";
            var lead = UpdateStatusAndHistory(id, status, user, description);
            await UnitOfWork.SaveAsync();
            return lead;
        }

        public async Task<Lead> Cancel(int id, string user)
        {
            const int status = (int) LeadStatusTypes.Cancelled;
            const string description = "lead cancelled";
            var lead = UpdateStatusAndHistory(id, status, user, description);
            await UnitOfWork.SaveAsync();
            return lead;
        }

        public async Task<Lead> UpdateQp(int id, string user, UpdateQpModel updateQpModel)
        {
            const int status = (int) LeadStatusTypes.New;
            var qp = UnitOfWork.LeadPersonalRepository.Get(updateQpModel.QpId);
            string description = "change QP to " + qp.Initial + " - " + qp.Name;
            if (qp == null) throw new Exception("QP " + updateQpModel.QpId + " not found");
            var lead = UpdateStatusAndHistory(id, status, user, description);
            lead.LeadPersonal = qp;
            await UnitOfWork.SaveAsync();
            var newLead = Mapper.Map<NewLead>(lead);
            newLead.Url = updateQpModel.Url;
            newLead.Email = _emailHelper.GetEmailByInitial(lead.LeadPersonal.Initial);
            await DomainEvents.Raise(newLead);
            return lead;
        }

        public async Task<List<LeadPersonal>> GetAllQpByZone(string zone)
        {
            var list =
                UnitOfWork.AllocationRepository.Get(l => l.Zone == zone)
                    .Select(l => l.LeadPersonal)
                    .OrderBy(l => l.Initial)
                    .Distinct()
                    .ToList();
            //Always include BDGMInital
            if (list.Any(l => l.Initial == _settings.BDGMInitial)) return list;
            var special =
                await UnitOfWork.LeadPersonalRepository.SingleOrDefaultAsync(l => l.Initial == _settings.BDGMInitial);
            if (special == null) throw new Exception("BDGM initail cannot be found");
            list.Add(special);
            return list;
        }

        public Lead NewLead(int contactId, int? leadPersonId, int? telesaleId)
        {
            LeadPersonal leadPerson = null;
            Telesale telesale = null;

            if (contactId <= 0)
                throw new Exception("Invalid contact id");

            var contact = UnitOfWork.ContactRepository.Get(contactId);
            if (contact == null)
                throw new Exception("Target contact not found in database");

            if (leadPersonId.HasValue)
                leadPerson = UnitOfWork.LeadPersonalRepository.Get(leadPersonId);

            if (telesaleId.HasValue)
                telesale = UnitOfWork.TelesaleRepository.Get(telesaleId);

            var assembler = new LeadAssembler(contact, leadPerson, telesale, new LeadFactory());
            var leadType = (BusinessTypes) Enum.Parse(typeof (BusinessTypes), contact.BusinessTypeId.ToString());
            var lead = assembler.Assemble(leadType);
            UnitOfWork.LeadRepository.Add(lead);
            UnitOfWork.Save();
            return lead;
        }

        private Lead UpdateStatusAndHistory(int id, int statusId, string user, string description)
        {
            var lead = UnitOfWork.LeadRepository.Get(id);
            var leadStatus = UnitOfWork.GetRepository<LeadStatus>().Get(statusId);
            if (lead == null) throw new Exception("Lead " + id + " not found");
            if (leadStatus == null) throw new Exception("Lead status " + statusId + " not found");
           
            lead.LastUpdateDate = DateTime.Now;
            UnitOfWork.GetRepository<LeadHistory>().Add(new LeadHistory
            {
                Description = description,
                Time = DateTime.Now,
                User = user,
                FromStatusId = lead.LeadStatusId,
                ToStatusId = statusId,
                LeadId = id
            });
            lead.LeadStatus = leadStatus;
            return lead;
        }


        private Quote CreateQuote(Lead lead)
        {
            var quote = new Quote
            {
                LeadId = lead.Id,
                LeadPersonalId = lead.LeadPersonalId,
                StatusId = (int)QuoteStatusTypes.New,
                BusinessTypeId = lead.BusinessType.Id,
                CreatedDate = DateTime.Now,
                LastUpdateDate = DateTime.Now,
                Company = lead.Contact.Site.Name,
                Address = new Address
                {
                    Number = lead.Contact.Site.Number,
                    Street = lead.Contact.Site.Street,
                    Suburb = lead.Contact.Site.Suburb,
                    Unit = lead.Contact.Site.Unit
                },
                State = lead.Contact.Site.State,
                Postcode = lead.Contact.Site.Postcode,
                SiteId = lead.Contact.SiteId,
                Phone = lead.Phone,
                Firstname = lead.Contact.ContactPerson.Firstname,
                Lastname = lead.Contact.ContactPerson.Lastname,
                Title = lead.Contact.ContactPerson.Title,
                Position = lead.Contact.ContactPerson.Position,
            };
            return quote;
        }
    }
}