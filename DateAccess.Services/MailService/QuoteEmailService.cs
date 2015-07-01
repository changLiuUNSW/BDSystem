using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;
using DataAccess.Common;
using DataAccess.Common.Email;
using DataAccess.EntityFramework;
using DataAccess.EntityFramework.Models.Quad;
using DataAccess.EntityFramework.Models.Quote;

namespace DateAccess.Services.MailService
{
    public interface IQuoteEmailService
    {
        Task SendCostsToWp(Quote quote, string url);
        Task SendMergedQuote(Quote quote, string url, List<string> to);
        Task SendIssueAlert(Quote quote,string issueDetail, string url, List<string> to);
        Task SendPrintEmailToWp(Quote quote, string url);
        Task SendPresentClientAlertToQp(Quote quote,string url);
        Task SendQuoteDocumentToQp(Quote quote,string url,bool pricePage);
    }


    public class QuoteEmailService : IQuoteEmailService
    {
        private readonly EmailFactory _emailFactory;
        private readonly ApplicationSettings _settings;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailHelper _emailHelper;

        public QuoteEmailService(EmailFactory emailFactory, ApplicationSettings settings,IUnitOfWork unitOfWork,IEmailHelper emailHelper)
        {
            _emailFactory = emailFactory;
            _settings = settings;
            _unitOfWork = unitOfWork;
            _emailHelper = emailHelper;
        }

        public Task SendCostsToWp(Quote quote, string url)
        {
            var toList = new List<string>();
            var templatePath = AppDomain.CurrentDomain.BaseDirectory + _settings.QuoteCostsEmailPath;
            var rootPath = AppDomain.CurrentDomain.BaseDirectory + _settings.CostUploadPath + @"/" + quote.Id;
            var email = _emailFactory.GetNewEmail();
            toList.Add(_settings.WPEmail);
            email.To.AddRange(toList);
            email.Subject = string.Format("Cost estimations for Quote {0}: {1}",quote.Id,quote.Company);
            var tokens = new Dictionary<string, string>
            {
                {"@quoteId", quote.Id.ToString(CultureInfo.InvariantCulture)},
                {"@quoteType", quote.BusinessType.Type},
                {"@company", quote.Company},
                {"@pw",quote.TotalPW==null?"N/A":quote.TotalPW.Value.ToString("0.00")},
                {"@pa", quote.TotalPA==null?"N/A":quote.TotalPA.Value.ToString("0.00")},
                {"@address", string.Format("{0} {1} {2}", quote.Address.Unit, quote.Address.Number, quote.Address.Street)},
                {"@suburb", quote.Address.Suburb},
                {"@state", quote.State},
                {"@postcode", quote.Postcode},
                {"@url", url}
            };
            //Add attachment
            foreach (var cost in quote.QuoteCost)
            {
                var fileName = Path.Combine(rootPath, cost.FileName);
                var attachment = new Attachment(fileName, MediaTypeNames.Application.Octet);
                ContentDisposition disposition = attachment.ContentDisposition;
                disposition.CreationDate = File.GetCreationTime(fileName);
                disposition.ModificationDate = File.GetLastWriteTime(fileName);
                disposition.ReadDate = File.GetLastAccessTime(fileName);
                disposition.FileName = Path.GetFileName(fileName);
                disposition.Size = new FileInfo(fileName).Length;
                disposition.DispositionType = DispositionTypeNames.Attachment;
                email.Attachments.Add(attachment);                
            }

            return email.SendEmail(templatePath, tokens);
        }

        public Task SendPrintEmailToWp(Quote quote, string url)
        {
            var email = _emailFactory.GetNewEmail();
            var templatePath = AppDomain.CurrentDomain.BaseDirectory + _settings.QuotePrintEmailPath;
            var rootPath = AppDomain.CurrentDomain.BaseDirectory + _settings.QuoteUploadPath + @"/" + quote.Id;
            var toList = new List<string> {_settings.WPEmail};
            email.To.AddRange(toList);
            email.Subject = string.Format("Ready for print  Quote {0}: {1}", quote.Id, quote.Company);
            var qp = _unitOfWork.GetRepository<QuadPhoneBook>().SingleOrDefault(l => l.Intial == quote.LeadPersonal.Initial);
            var tokens = new Dictionary<string, string>
            {
                {"@qpName",qp==null?quote.LeadPersonal.Initial:qp.Firstname+" "+qp.Lastname},
                {"@quoteId", quote.Id.ToString(CultureInfo.InvariantCulture)},
                {"@quoteType", quote.BusinessType.Type},
                {"@company", quote.Company},
                {"@pw",quote.TotalPW==null?"N/A":quote.TotalPW.Value.ToString("0.00")},
                {"@pa", quote.TotalPA==null?"N/A":quote.TotalPA.Value.ToString("0.00")},
                {"@address", string.Format("{0} {1} {2}", quote.Address.Unit, quote.Address.Number, quote.Address.Street)},
                {"@suburb", quote.Address.Suburb},
                {"@state", quote.State},
                {"@postcode", quote.Postcode},
                {"@url", url}
            };
            //Add attachment
            var fileName = Path.Combine(rootPath, quote.FileName);
            var attachment = new Attachment(fileName, MediaTypeNames.Application.Octet);
            ContentDisposition disposition = attachment.ContentDisposition;
            disposition.CreationDate = File.GetCreationTime(fileName);
            disposition.ModificationDate = File.GetLastWriteTime(fileName);
            disposition.ReadDate = File.GetLastAccessTime(fileName);
            disposition.FileName = Path.GetFileName(fileName);
            disposition.Size = new FileInfo(fileName).Length;
            disposition.DispositionType = DispositionTypeNames.Attachment;
            email.Attachments.Add(attachment);
            return email.SendEmail(templatePath, tokens);
        }

        public Task SendPresentClientAlertToQp(Quote quote, string url)
        {
            var templatePath = AppDomain.CurrentDomain.BaseDirectory + _settings.QuotePresentClientEmailPath;
            var email = _emailFactory.GetNewEmail();
            var toEmail = _emailHelper.GetEmailByInitial(quote.LeadPersonal.Initial);
            var rootPath = AppDomain.CurrentDomain.BaseDirectory + _settings.QuoteUploadPath + @"/" + quote.Id;
            email.To.Add(toEmail);
            email.Subject = string.Format("Please present to the client Quote {0}: {1}", quote.Id, quote.Company);
            var tokens = new Dictionary<string, string>
            {
                {"@completeDate",quote.PrintDate.Value.ToString("dd/MM/yyyy")},
                {"@quoteId", quote.Id.ToString(CultureInfo.InvariantCulture)},
                {"@quoteType", quote.BusinessType.Type},
                {"@company", quote.Company},
                {"@address", string.Format("{0} {1} {2}", quote.Address.Unit, quote.Address.Number, quote.Address.Street)},
                {"@suburb", quote.Address.Suburb},
                {"@state", quote.State},
                {"@postcode", quote.Postcode},
                {"@pw",quote.TotalPW==null?"N/A":quote.TotalPW.Value.ToString("0.00")},
                {"@pa", quote.TotalPA==null?"N/A":quote.TotalPA.Value.ToString("0.00")},
                {"@url", url}
            };
            //Add attachment
            var attachmentFiles = new List<string>();
            if (!string.IsNullOrEmpty(quote.FileName)) attachmentFiles.Add(quote.FileName);
            if (!string.IsNullOrEmpty(quote.PricePageName)) attachmentFiles.Add(quote.PricePageName);
            foreach (var file in attachmentFiles)
            {
                var fileName = Path.Combine(rootPath, file);
                var attachment = new Attachment(fileName, MediaTypeNames.Application.Octet);
                ContentDisposition disposition = attachment.ContentDisposition;
                disposition.CreationDate = File.GetCreationTime(fileName);
                disposition.ModificationDate = File.GetLastWriteTime(fileName);
                disposition.ReadDate = File.GetLastAccessTime(fileName);
                disposition.FileName = Path.GetFileName(fileName);
                disposition.Size = new FileInfo(fileName).Length;
                disposition.DispositionType = DispositionTypeNames.Attachment;
                email.Attachments.Add(attachment);
            }
            return email.SendEmail(templatePath, tokens);
        }

        public Task SendQuoteDocumentToQp(Quote quote, string url, bool pricePage)
        {
            var templatePath = AppDomain.CurrentDomain.BaseDirectory + _settings.QuoteDocumentEmailPath;
            var email = _emailFactory.GetNewEmail();
            var toEmail = _emailHelper.GetEmailByInitial(quote.LeadPersonal.Initial);
            var rootPath = AppDomain.CurrentDomain.BaseDirectory + _settings.QuoteUploadPath + @"/" + quote.Id;
            email.To.Add(toEmail);
            email.Subject = string.Format(pricePage ? "Price Page for Quote {0}: {1}" : "Quote Document for Quote {0}: {1}", quote.Id, quote.Company);
            var tokens = new Dictionary<string, string>
            {
                {"@documentType",pricePage?"price page":"quote document"},
                {"@quoteId", quote.Id.ToString(CultureInfo.InvariantCulture)},
                {"@quoteType", quote.BusinessType.Type},
                {"@company", quote.Company},
                {"@address", string.Format("{0} {1} {2}", quote.Address.Unit, quote.Address.Number, quote.Address.Street)},
                {"@suburb", quote.Address.Suburb},
                {"@state", quote.State},
                {"@postcode", quote.Postcode},
                {"@pw",quote.TotalPW==null?"N/A":quote.TotalPW.Value.ToString("0.00")},
                {"@pa", quote.TotalPA==null?"N/A":quote.TotalPA.Value.ToString("0.00")},
                {"@url", url}
            };
            var attachmentFiles = new List<string>();
            if (!pricePage&&!string.IsNullOrEmpty(quote.FileName)) attachmentFiles.Add(quote.FileName);
            if (pricePage&&!string.IsNullOrEmpty(quote.PricePageName)) attachmentFiles.Add(quote.PricePageName);
            foreach (var file in attachmentFiles)
            {
                var fileName = Path.Combine(rootPath, file);
                var attachment = new Attachment(fileName, MediaTypeNames.Application.Octet);
                ContentDisposition disposition = attachment.ContentDisposition;
                disposition.CreationDate = File.GetCreationTime(fileName);
                disposition.ModificationDate = File.GetLastWriteTime(fileName);
                disposition.ReadDate = File.GetLastAccessTime(fileName);
                disposition.FileName = Path.GetFileName(fileName);
                disposition.Size = new FileInfo(fileName).Length;
                disposition.DispositionType = DispositionTypeNames.Attachment;
                email.Attachments.Add(attachment);
            }
            return email.SendEmail(templatePath, tokens);
        }

        public Task SendMergedQuote(Quote quote, string url, List<string> to)
        {
            var email = _emailFactory.GetNewEmail();
            var templatePath = AppDomain.CurrentDomain.BaseDirectory + _settings.QuoteMergedEmailPath;
            var rootPath = AppDomain.CurrentDomain.BaseDirectory + _settings.QuoteUploadPath + @"/" + quote.Id;
            email.To.AddRange(to);
            email.Subject = string.Format("Please Review  Quote {0}: {1}", quote.Id, quote.Company);
            var pa = quote.QuoteCost.Where(l => l.PricePa != null).Sum(l => l.PricePa);
            var pw = quote.QuoteCost.Where(l => l.ReturnPw != null).Sum(l => l.ReturnPw);
            var tokens = new Dictionary<string, string>
            {
                {"@quoteId", quote.Id.ToString(CultureInfo.InvariantCulture)},
                {"@quoteType", quote.BusinessType.Type},
                {"@company", quote.Company},
                {"@pw",pa==null?"N/A":pw.Value.ToString("0.00")},
                {"@pa", pw==null?"N/A":pa.Value.ToString("0.00")},
                {"@address", string.Format("{0} {1} {2}", quote.Address.Unit, quote.Address.Number, quote.Address.Street)},
                {"@suburb", quote.Address.Suburb},
                {"@state", quote.State},
                {"@postcode", quote.Postcode},
                {"@url", url}
            };
            //Add attachment
         
                var fileName = Path.Combine(rootPath, quote.FileName);
                var attachment = new Attachment(fileName, MediaTypeNames.Application.Octet);
                ContentDisposition disposition = attachment.ContentDisposition;
                disposition.CreationDate = File.GetCreationTime(fileName);
                disposition.ModificationDate = File.GetLastWriteTime(fileName);
                disposition.ReadDate = File.GetLastAccessTime(fileName);
                disposition.FileName = Path.GetFileName(fileName);
                disposition.Size = new FileInfo(fileName).Length;
                disposition.DispositionType = DispositionTypeNames.Attachment;
                email.Attachments.Add(attachment);
            
            return email.SendEmail(templatePath, tokens);
        }

        public Task SendIssueAlert(Quote quote, string issueDetail, string url, List<string> to)
        {
            var templatePath = AppDomain.CurrentDomain.BaseDirectory + _settings.QuoteIssueAlertEmailPath;
            var email = _emailFactory.GetNewEmail();
            email.To.AddRange(to);
            email.Subject = string.Format("Alert New Issue for Quote {0}: {1}", quote.Id, quote.Company);
            var tokens = new Dictionary<string, string>
            {
                {"@quoteId", quote.Id.ToString(CultureInfo.InvariantCulture)},
                {"@quoteType", quote.BusinessType.Type},
                {"@company", quote.Company},
                {"@address", string.Format("{0} {1} {2}", quote.Address.Unit, quote.Address.Number, quote.Address.Street)},
                {"@suburb", quote.Address.Suburb},
                {"@state", quote.State},
                {"@postcode", quote.Postcode},
                {"@issueDetail", issueDetail},
                {"@url", url}
            };
            return email.SendEmail(templatePath, tokens);
        }
    }
}