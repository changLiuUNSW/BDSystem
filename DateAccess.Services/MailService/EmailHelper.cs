using System;
using System.Linq;
using DataAccess.Common;
using DataAccess.EntityFramework;
using DataAccess.EntityFramework.Models.Quad;

namespace DateAccess.Services.MailService
{
    public interface IEmailHelper
    {
        string GetEmailByInitial(string initial);
    }

    internal class EmailHelper : IEmailHelper
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationSettings _settings;


        public EmailHelper(IUnitOfWork unitOfWork, ApplicationSettings settings)
        {
            _unitOfWork = unitOfWork;
            _settings = settings;
        }

        public string GetEmailByInitial(string initial)
        {
            var qpPhoneBook =
           _unitOfWork.GetRepository<QuadPhoneBook>()
               .Get(l => l.Intial == initial)
               .FirstOrDefault();
            if (qpPhoneBook != null && !string.IsNullOrEmpty(qpPhoneBook.Email))
            {
                return qpPhoneBook.Email;
            }
            return _settings.AdminEmail;
        }
    }
}