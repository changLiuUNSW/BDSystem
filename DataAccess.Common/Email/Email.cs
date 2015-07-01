using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using log4net;

namespace DataAccess.Common.Email
{
    public interface IEmail
    {
        string SmtpHost { get; set; }
        string From { get; set; }
        string FromDisplayName { get; set; }
        string Subject { get; set; }
        string Body { get; set; }
        bool IsTextOnly { get; set; }
        List<string> To { get; }
        List<string> Cc { get; }
        List<string> Bcc { get; }
        List<Attachment> Attachments { get; }
        Task SendEmail();
        Task SendEmail(string templateFilePath, Dictionary<string, string> tokens);
        void SaveEmailToFile(string fileName);
        void SaveEmailToFile(string fileName, string templateFilePath, Dictionary<string, string> tokens);
    }

    internal class Email : IEmail
    {
        private const string SentOnKey = "@sentOn";
        private const string ServerNameKey = "@server";
        private const string SystemName = "BD System";
        private readonly IEmailSender _emailSender;
        private readonly ILog _log;

        public Email(ILog log, IEmailSender emailSender)
        {
            if (emailSender == null) throw new ArgumentException("emailSender cannot be null");

            _log = log;
            _emailSender = emailSender;

            To = new List<string>();
            Cc = new List<string>();
            Bcc = new List<string>();
            Attachments = new List<Attachment>();
        }

        public string SmtpUsername { get; set; }
        public string SmtpPassword { get; set; }
        public string SmtpHost { get; set; }
        public string From { get; set; }
        public string FromDisplayName { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsTextOnly { get; set; }
        public List<string> To { get; private set; }
        public List<string> Cc { get; private set; }
        public List<string> Bcc { get; private set; }
        public List<Attachment> Attachments { get; private set; }

        public Task SendEmail()
        {
            return SendEmailImpl();
        }


        public Task SendEmail(string templateFilePath, Dictionary<string, string> tokens)
        {
            return SendEmailImpl(templateFilePath, tokens);
        }


        public void SaveEmailToFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) throw new ArgumentNullException("fileName");

            using (var mm = new MailMessage())
            {
                PopulateMailMessage(mm, null, null);
                EmailFileSaver.SaveMailMessageToFile(mm, fileName);
            }
        }

        public void SaveEmailToFile(string fileName, string templateFilePath, Dictionary<string, string> tokens)
        {
            if (string.IsNullOrEmpty(fileName)) throw new ArgumentNullException("fileName");

            using (var mm = new MailMessage())
            {
                PopulateMailMessage(mm, templateFilePath, tokens);
                EmailFileSaver.SaveMailMessageToFile(mm, fileName);
            }
        }

        private Task SendEmailImpl(string bodyTemplateFilePath = null,
            Dictionary<string, string> tokens = null)
        {
            if (_emailSender == null) throw new ArgumentException("emailSender cannot be empty");
            if (string.IsNullOrEmpty(From)) throw new ArgumentException("From email cannot be empty");
            if (To == null || !To.Any()) throw new ArgumentException("To emails cannot be empty");
            if (!string.IsNullOrEmpty(bodyTemplateFilePath) && !File.Exists(bodyTemplateFilePath))
            {
                throw new ArgumentException(
                    "The provided path for the body template (argument 'BodyTemplateFilePath') does not exist or access is denied.");
            }
            var mm = new MailMessage();
            PopulateMailMessage(mm, bodyTemplateFilePath, tokens);
            // Ensure this email object cannot be used again!
            To.Clear();
            From = null;
            return _emailSender.SendEmail(SmtpHost, SmtpUsername, SmtpPassword, mm);
        }

        private void PopulateMailMessage(MailMessage mm, string bodyTemplateFilePath, Dictionary<string, string> tokens)
        {
            ValidateSubjectLength();

            mm.IsBodyHtml = !IsTextOnly;
            if (!String.IsNullOrEmpty(From))
            {
                mm.From = new MailAddress(From, FromDisplayName ?? String.Empty);
            }
            mm.Subject = Subject.Replace("\n", " ").Replace("\r", " ");
            AddMessageEmailAddressToCollection(To, mm.To);
            AddMessageEmailAddressToCollection(Cc, mm.CC);
            AddMessageEmailAddressToCollection(Bcc, mm.Bcc);
            AddMessageAttachments(mm);
            AddMessageBody(mm, bodyTemplateFilePath, tokens);
        }

        private void ValidateSubjectLength()
        {
            if (String.IsNullOrEmpty(Subject))
            {
                Subject = "No Subject";
            }
            else if (Subject.Length > 900)
            {
                Subject = Subject.Substring(0, 900);
            }
        }

        private void AddMessageEmailAddressToCollection(IEnumerable<string> addresses, MailAddressCollection cc)
        {
            if (addresses == null) throw new ArgumentNullException("addresses");
            if (cc == null) throw new ArgumentNullException("cc");

            foreach (string address in addresses.Where(a => !String.IsNullOrEmpty(a)))
            {
                try
                {
                    cc.Add(address);
                }
                catch (FormatException e)
                {
                    _log.Info("Not adding: " + address + " to the email.  It appears to be an invalid address. " +
                              e.Message);
                }
            }
        }

        private void AddMessageAttachments(MailMessage mm)
        {
            if (mm == null) throw new ArgumentNullException("mm");
            if (Attachments == null) throw new ArgumentException("Attachements cannot be empty");
            foreach (Attachment a in Attachments)
            {
                mm.Attachments.Add(a);
            }
        }

        private void AddMessageBody(MailMessage mm, string bodyTemplateFilePath, Dictionary<string, string> tokens)
        {
            if (mm == null) throw new ArgumentNullException("mm");
            if (!string.IsNullOrEmpty(bodyTemplateFilePath))
            {
                string template = LoadTemplate(bodyTemplateFilePath);
                if (tokens != null && tokens.Any())
                {
                    if (!tokens.ContainsKey(SentOnKey))
                        tokens.Add(SentOnKey, DateTime.Now.ToShortDateString());
                    if (!tokens.ContainsKey(ServerNameKey))
                        tokens.Add(ServerNameKey, SystemName);
                    template = ProcessTokens(template, tokens);
                }
                mm.Body = template;
            }
            else
            {
                mm.Body = Body;
            }
        }

        private string ProcessTokens(string template, Dictionary<string, string> tokens)
        {
            foreach (string key in tokens.Keys)
            {
                template = template.Replace(key, tokens[key]);
            }
            return template;
        }


        private string LoadTemplate(string bodyTemplateFilePath)
        {
            using (StreamReader re = File.OpenText(bodyTemplateFilePath))
            {
                return re.ReadToEnd();
            }
        }
    }
}