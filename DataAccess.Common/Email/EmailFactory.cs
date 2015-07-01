using System;
using log4net;

namespace DataAccess.Common.Email
{
    public class EmailFactory
    {
        private readonly string _bcc;
        private readonly IEmailSender _emailSender;
        private readonly string _from;
        private readonly ILog _log;
        private readonly string _password;
        private readonly string _smtp;
        private readonly string _username;

        public EmailFactory(ILog log, IEmailSender emailSender, string smtp, string from, string bcc = null,
            string username = null, string password = null)
        {
            if (emailSender == null) throw new ArgumentNullException("emailSender");

            _log = log;
            _emailSender = emailSender;
            _smtp = smtp;
            _from = from;
            _bcc = bcc;
            _username = username;
            _password = password;
        }

        public IEmail GetNewEmail()
        {
            var e = new Email(_log, _emailSender)
            {
                SmtpHost = _smtp,
                SmtpUsername = _username,
                SmtpPassword = _password,
                From = _from
            };
            if (!String.IsNullOrWhiteSpace(_bcc))
            {
                e.Bcc.AddRange(_bcc.Split(new[] { ',', ';' }));
            }
            return e;
        }
    }
}
