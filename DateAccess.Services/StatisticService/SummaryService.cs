using System;
using System.Collections.Generic;
using DataAccess.EntityFramework;
using DataAccess.EntityFramework.Repositories;

namespace DateAccess.Services.StatisticService
{
    public interface ISummaryService
    {
        /// <summary>
        /// return total row count in contact table for each distinct business types
        /// </summary>
        /// <returns></returns>
        List<SummaryCount> SiteCount();

        /// <summary>
        /// similar to SiteCount() except it takes a bd username and limit the count for those only belongs to the bd
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        List<SummaryCount> SiteCountForBD(string userName);


        /// <summary>
        /// return total row count for contact that has a valid contact person in contact table for each distinct business types
        /// </summary>
        /// <returns></returns>
        List<SummaryCount> ContactPersonCount();

        /// <summary>
        /// similar to ContactPersonCount() except it takes a bd username and limit the count to those only belongs to the BD
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        List<SummaryCount> ContactPersonCountForBD(string userName);
    }

    public class SummaryService : ISummaryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SummaryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<SummaryCount> SiteCount()
        {
            return _unitOfWork.ContactRepository.BusinessTypeCount();
        }

        /// <summary>
        /// extract first two letters of the user name when counting
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public List<SummaryCount> SiteCountForBD(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                return new List<SummaryCount>();

            return _unitOfWork.ContactRepository.BusinessTypeCount(GetInitial(userName).Substring(0, 2));
        }

        public List<SummaryCount> ContactPersonCount()
        {
            return _unitOfWork.ContactRepository.ContactPersonCount();
        }

        /// <summary>
        /// extract first two letters of the user name when counting
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public List<SummaryCount> ContactPersonCountForBD(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                return new List<SummaryCount>();

            return _unitOfWork.ContactRepository.ContactPersonCount(GetInitial(userName).Substring(0, 2));
        }

        private string GetInitial(string userName)
        {
            var person = _unitOfWork.LeadPersonalRepository.GetFromPhoneBook(userName);
            if (person == null)
                throw new Exception("The current user initial was not found.");

            return person.Initial;
        }
    }
}
