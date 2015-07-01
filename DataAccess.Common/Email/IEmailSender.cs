using System.Net.Mail;
using System.Threading.Tasks;

namespace DataAccess.Common.Email
{
    public interface IEmailSender
    {
        Task SendEmail(string smtpHost, string username, string password, MailMessage mailMessage);
    }
}
