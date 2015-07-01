using System;
using System.ComponentModel;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using log4net;

namespace DataAccess.Common.Email
{
    public class EmailSender : IEmailSender
    {
        private readonly ILog _log;
        private readonly ApplicationSettings _settings;
        private MailMessage _message;
        private SmtpClient _smtp;

        public EmailSender(ILog log, ApplicationSettings settings)
        {
            _log = log;
            _settings = settings;
        }

        public Task SendEmail(string smtpHost, string username, string password, MailMessage mailMessage)
        {
            if (string.IsNullOrEmpty(smtpHost)) throw new ArgumentNullException("smtpHost");
            if (mailMessage == null) throw new ArgumentNullException("mailMessage");
            if (!string.IsNullOrEmpty(_settings.TestingOverrideEmail))
            {
                mailMessage.To.Clear();
                mailMessage.Bcc.Clear();
                mailMessage.CC.Clear();
                mailMessage.To.Add(_settings.TestingOverrideEmail);
            }
            _message = mailMessage;
            _smtp = new SmtpClient(smtpHost, 25);
            if (!String.IsNullOrEmpty(username))
            {
                _smtp.Credentials = new NetworkCredential(username, password);
                _smtp.UseDefaultCredentials = false;
            }
            if (_log != null)
            {
                _log.Info(String.Format("Sending Email[{0}] To[{1}] Cc[{2}] Bcc[{3}]",
                    _message.Subject,
                    String.Join(", ", _message.To),
                    String.Join(", ", _message.CC),
                    String.Join(", ", _message.Bcc)));
            }
            _smtp.SendCompleted += AsyncSendCompleted;
            return _smtp.SendMailAsync(_message);
        }


        private void AsyncSendCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                _log.Error("Email Send canceled");
            }
            if (e.Error != null)
            {
                _log.Error("Error Sending Email: " + e.Error);
            }
            DisposeMessage();
        }

        private void DisposeMessage()
        {
            if (_message != null)
            {
                _message.Dispose();
                _message = null;
            }

            if (_smtp != null)
            {
                _smtp.Dispose();
                _smtp = null;
            }
        }
    }
}