using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using DataAccess.Common;
using DataAccess.Common.Email;
using DataAccess.EntityFramework.Models.BD.Lead;
using DataAccess.EntityFramework.TypeLibrary;

namespace DateAccess.Services.MailService
{
    public interface ILeadEmailService
    {
        Task SendAppointmentEmail(Lead lead, string url);
        Task SendNewLeadEmail(Lead lead, string url);
    }

    internal class LeadEmailService : ILeadEmailService
    {
        private readonly EmailFactory _emailFactory;
        private readonly ApplicationSettings _settings;
        private readonly IEmailHelper _emailHelper;
        public LeadEmailService(EmailFactory emailFactory, ApplicationSettings settings,IEmailHelper emailHelper)
        {
            _emailFactory = emailFactory;
            _settings = settings;
            _emailHelper = emailHelper;
        }

        public Task SendAppointmentEmail(Lead lead, string url)
        {
            var templatePath = AppDomain.CurrentDomain.BaseDirectory + _settings.LeadAppointmentEmailPath;
            IEmail email = _emailFactory.GetNewEmail();
            var tolist = new List<string>();
            var qpEmail = _emailHelper.GetEmailByInitial(lead.LeadPersonal.Initial);
            tolist.Add(qpEmail);
            email.To.AddRange(tolist);
            email.Subject = "Appointment Confirmed: " + lead.Contact.Site.Name;
            var tokens = new Dictionary<string, string>
            {
                {"@name", lead.LeadPersonal.Name},
                {"@leadId", lead.Id.ToString(CultureInfo.InvariantCulture)},
                {"@leadType", lead.BusinessType.Type},
                {"@company", lead.Contact.Site.Name},
                {"@appointmentDate", lead.AppointmentDate.Value.ToShortDateString()},
                {"@phone", lead.Phone},
                {"@address", string.Format("{0} {1} {2}", lead.Address.Unit, lead.Address.Number, lead.Address.Street)},
                {"@suburb", lead.Address.Suburb},
                {"@state", lead.State},
                {"@postcode", lead.Postcode},
                {"@url", url}
            };
           return email.SendEmail(templatePath, tokens);
        }

        public Task SendNewLeadEmail(Lead lead, string url)
        {
            var templatePath = AppDomain.CurrentDomain.BaseDirectory + _settings.NewLeadAlertEmailPath;
            IEmail email = _emailFactory.GetNewEmail();
            var tolist = new List<string>();
            var qpEmail = _emailHelper.GetEmailByInitial(lead.LeadPersonal.Initial);
            tolist.Add(qpEmail);
            email.To.AddRange(tolist);
            email.Subject = "New Lead: " + lead.Contact.Site.Name;

            var tokens = new Dictionary<string, string>
            {
                {"@name", lead.LeadPersonal.Name},
                {"@leadId", lead.Id.ToString(CultureInfo.InvariantCulture)},
                {"@leadType", ((BusinessTypes)lead.BusinessTypeId).ToString()},
                {"@company", lead.Contact.Site.Name},
                {"@phone", lead.Phone},
                {"@address", string.Format("{0} {1} {2}", lead.Address.Unit, lead.Address.Number, lead.Address.Street)},
                {"@suburb", lead.Address.Suburb},
                {"@state", lead.State},
                {"@postcode", lead.Postcode},
                {"@url", url}
            };
            return email.SendEmail(templatePath, tokens);
        }

   
    }
}