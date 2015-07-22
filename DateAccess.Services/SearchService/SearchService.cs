using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using DataAccess.Common.SearchModels;
using DataAccess.EntityFramework;
using DataAccess.EntityFramework.Models.BD.Contact;
using DataAccess.EntityFramework.Models.BD.Site;
using DataAccess.EntityFramework.Repositories;
using DateAccess.Services.SearchService.Projector;
using DateAccess.Services.SearchService.Role;

namespace DateAccess.Services.SearchService
{
    /// <summary>
    /// Provide run-time filter options on database tables 
    /// </summary>
    public interface ISearchService
    {
        List<Site> SeachSiteAddr(string keyword,int? take);
        List<Site> SearchSiteBykey(string key, int? take);
        Task<SearchResult<LeadSearch>> SearchLead(LeadSearchModel leadSearchModel);
        Task<SearchResult<QuoteProgressModel>> SearchProgressQuote(QuoteProgressSearchModel model);
        Task<SearchResult<QuoteCurrentModel>> SearchCurrentQuote(QuoteCurrentSearchModel model); 
        Task<SearchResult<AdminSearch>> AdminSiteSearch(IPrincipal user, Search search);
        Task<SearchResult<AdminSearch>> AdminContactPersonSearch(IPrincipal user, Search search);

        Task<SearchResult<T>> Search<T>(SearchConfiguration<T> configuration) where T : class;
    }

    internal class SearchService : ISearchService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SearchService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<SearchResult<AdminSearch>> AdminSiteSearch(IPrincipal user, Search search)
        {
            var configuration = new SearchConfiguration<Site>(search);
            configuration.SetSortingOrderIfNotExist("key", SearchSortingOrder.Ascending);
            var field =
                search.SearchFields.SingleOrDefault(
                    x => string.Compare(x.Field, "BusinessType", StringComparison.OrdinalIgnoreCase) == 0);
            
            configuration.Projector = new AdminSiteProjector
            {
                BusinessType = field != null ? field.Term : null
            };

            if (field != null)
                configuration.Predicates.Add(SearchExpressionLibrary.MatchBusinessTypeForSite(field.Term));

            ConfigureRoleSpecificSearch(user, configuration, field);
            return Search<Site, AdminSearch>(configuration);
        }

        public Task<SearchResult<AdminSearch>> AdminContactPersonSearch(IPrincipal user, Search search)
        {
            var configuration = new SearchConfiguration<Contact>(search);
            configuration.SetSortingOrderIfNotExist("key", SearchSortingOrder.Ascending);
            var field =
                search.SearchFields.SingleOrDefault(
                    x => string.Compare(x.Field, "BusinessType", StringComparison.OrdinalIgnoreCase) == 0);

            configuration.Projector = new AdminContactProjector();
            configuration.Predicates.Add(x=>x.ContactPersonId != null);
            ConfigureRoleSpecificSearch(user, configuration, field);
            return Search<Contact, AdminSearch>(configuration);
        }

        private void ConfigureRoleSpecificSearch<T>(IPrincipal user, SearchConfiguration<T> configuration, SearchField businessType) where T : class
        {
            if (user.IsInRole("BD"))
            {
                var initial = _unitOfWork.LeadPersonalRepository.GetFromPhoneBook(user.Identity.Name).Initial;
                var bdAdapter = new BDSearchAdapter(initial);

                if (businessType != null)
                    bdAdapter.BusinessType = businessType.Term;

                bdAdapter.Configure(configuration);
            }
        }

        /// <summary>
        /// search from the base table, projection in search configuration is ignored in this function
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public Task<SearchResult<T>> Search<T>(SearchConfiguration<T> configuration) where T: class
        {
            if (configuration == null)
                throw new Exception("Search configuration required");

            if (configuration.SearchParams == null)
                throw new Exception("Search configuration parameters required");

            var searchParameters = configuration.SearchParams;
            var predicates = configuration.Predicates;

            return _unitOfWork.GetRepository<T>().SearchAsync(searchParameters, predicates.ToArray());
        }

        /// <summary>
        /// search from a projection that is different from the base table
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public Task<SearchResult<TResult>> Search<T, TResult>(SearchConfiguration<T> configuration) 
            where T : class
            where TResult : class
        {
            if (configuration == null)
                throw new Exception("Search configuration required");

            if (configuration.SearchParams == null)
                throw new Exception("Search configuration parameters required");

            if (configuration.Projector == null)
                throw new Exception("Search projection type required");

            var searchParameters = configuration.SearchParams;
            var predicates = configuration.Predicates;
            var projectorExpression = configuration.Projector.CreateExpression<T, TResult>();

            return _unitOfWork.GetRepository<T>().SearchAsync(searchParameters, projectorExpression, predicates.ToArray());
        }

        /***
         * Functions below are to be removed after generic search functions are implemented 
         ***/
        public List<Site> SeachSiteAddr(string keyword , int? take)
        {
            return _unitOfWork.SiteRepository.SearchSiteAddr(keyword, take);
        }

        public Task<SearchResult<LeadSearch>> SearchLead(LeadSearchModel leadSearchModel)
        {
            return _unitOfWork.LeadRepository.SearchLead(leadSearchModel);
        }

        public Task<SearchResult<QuoteProgressModel>> SearchProgressQuote(QuoteProgressSearchModel model)
        {
            return _unitOfWork.QuoteRepository.SearchProgressQuote(model);
        }

        public Task<SearchResult<QuoteCurrentModel>> SearchCurrentQuote(QuoteCurrentSearchModel model)
        {
            return _unitOfWork.QuoteRepository.SearchCurrentQuote(model);
        }

        /***
         * END REMOVE
         ***/

        public List<Site> SearchSiteBykey(string key, int? take)
        {
            return _unitOfWork.SiteRepository.GetSiteKey(key, take);
        }
    }
}
