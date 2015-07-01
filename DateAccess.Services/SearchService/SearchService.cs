using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Common.SearchModels;
using DataAccess.EntityFramework;
using DataAccess.EntityFramework.Models.BD.Site;
using DataAccess.EntityFramework.Repositories;

namespace DateAccess.Services.SearchService
{
    public interface ISearchService
    {
        Task<SearchResult<AdminSearch>> SearchPerson(Search search);
        Task<SearchResult<AdminSearch>> SearchSite(Search search);
        Task<SearchResult<LeadSearch>> SearchLead(LeadSearchModel leadSearchModel);
        Task<SearchResult<QuoteProgressModel>> SearchProgressQuote(QuoteProgressSearchModel model);
        Task<SearchResult<QuoteCurrentModel>> SearchCurrentQuote(QuoteCurrentSearchModel model); 
        List<Site> SearchSiteBykey(string key = null, int? take = null);

    }
    internal class SearchService : ISearchService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SearchService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<SearchResult<AdminSearch>> SearchPerson(Search search)
        {
            var bizType=search.SearchFields.Single(l => l.Field == "BusinessType").Term;
            if (bizType.ToLower() == "other") return await _unitOfWork.ContactPersonRepository.PersonWithoutContactSearch(search);
            return await _unitOfWork.ContactRepository.Search(search);
        }
        public Task<SearchResult<AdminSearch>> SearchSite(Search search)
        {
            var bizType = search.SearchFields.Single(l => l.Field == "BusinessType").Term;
            return _unitOfWork.SiteRepository.Search(search, bizType);
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

        public List<Site> SearchSiteBykey(string key = null, int? take = null)
        {
            return _unitOfWork.SiteRepository.GetSiteKey(key, take);
        }
    }

   
}
