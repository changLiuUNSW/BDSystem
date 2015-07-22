using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using DataAccess.Common;
using DataAccess.Common.Email;
using DateAccess.Services.Events;
using DateAccess.Services.Infrastructure;
using log4net;

namespace DateAccess.Services.EventHandlers
{
    public class SendNewLeadEmail: IHandle<NewLead>
    {
        private readonly EmailFactory _emailFactory;
        private readonly ILog _log;
        private readonly ApplicationSettings _settings;

        public SendNewLeadEmail(EmailFactory emailFactory, ApplicationSettings settings, ILog log)
        {
            _emailFactory = emailFactory;
            _settings = settings;
            _log = log;
        }

        public async Task Handle(NewLead domainEvent)
        {
            try
            {
                string templatePath = AppDomain.CurrentDomain.BaseDirectory + _settings.NewLeadAlertEmailPath;
                IEmail email = _emailFactory.GetNewEmail();
                email.To.Add(domainEvent.Email);
                email.Subject = "New Lead: " + domainEvent.Company;
                var tokens = new Dictionary<string, string>
                {
                    {"@name", domainEvent.Name},
                    {"@leadId", domainEvent.LeadId.ToString(CultureInfo.InvariantCulture)},
                    {"@leadType", domainEvent.LeadType},
                    {"@company", domainEvent.Company},
                    {"@phone", domainEvent.Phone},
                    {"@address", string.Format("{0} {1} {2}", domainEvent.Unit, domainEvent.Number, domainEvent.Street)},
                    {"@suburb", domainEvent.Suburb},
                    {"@state", domainEvent.State},
                    {"@postcode", domainEvent.Postcode},
                    {"@url", domainEvent.Url}
                };
                await email.SendEmail(templatePath, tokens);
            }
            catch (Exception ex)
            {
                _log.Error("SendNewLeadEmail.Handle::NewLead:" + domainEvent, ex);
            }
        }
    }
}
