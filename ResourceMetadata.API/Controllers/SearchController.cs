using System.Threading.Tasks;
using System.Web.Http;
using DataAccess.Common.SearchModels;
using DataAccess.EntityFramework.Models.BD.Site;
using DataAccess.EntityFramework.Repositories;
using DateAccess.Services.SearchService;

namespace ResourceMetadata.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    [RoutePrefix("api/search")]
    public class SearchController : ApiController
    {

        private readonly ISearchService _searchService;

        /// <summary>
        ///     default constructor
        /// </summary>
        /// <param name="searchService"></param>
        public SearchController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        /// <summary>
        /// SearchPerson
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        [Route("person")]
        [HttpPost]
        public async Task<IHttpActionResult> SearchPerson(Search search)
        {
            var result = await _searchService.AdminContactPersonSearch(User, search);
            return Ok(new
            {
                data = result
            });
        }

        /// <summary>
        /// Search Site
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        [Route("site")]
        [HttpPost]
        public async Task<IHttpActionResult> SearchSite(Search search)
        {
            var result = await _searchService.AdminSiteSearch(User, search);
            return Ok(new
            {
                data = result
            });
        }

        /// <summary>
        /// return external manager table base on the search criteria
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        [Route("ExternalManager")]
        [HttpPost] 
        public async Task<IHttpActionResult> SearchExternalManager(Search search)
        {
            var config = new SearchConfiguration<ExternalManager>(search);
            config.SetSortingOrderIfNotExist("Id", SearchSortingOrder.Ascending);
            var result = await _searchService.Search(config);
            return Ok(new
            {
                data = result
            });
        }

        [Route("BuildingType")]
        [HttpPost]
        public async Task<IHttpActionResult> SearchBuildingType(Search search)
        {
            var config = new SearchConfiguration<BuildingType>(search);
            config.SetSortingOrderIfNotExist("Id", SearchSortingOrder.Ascending);
            var result = await _searchService.Search(config);
            return Ok(new
            {
                data = result
            });
        }
            
        [Route("address")]
        [HttpGet]
        public IHttpActionResult SearchSiteAddr(string keyword = null, int? take = null)
        {
            var results = _searchService.SeachSiteAddr(keyword, take);
            return Ok(new
            {
                data = results
            });
        }

        [Route("lead")]
        [HttpPost]
        public async Task<IHttpActionResult> SearchLead(LeadSearchModel leadSearchModel)
        {
            var results = await _searchService.SearchLead(leadSearchModel);
            return Ok(new
            {
                data = results
            });
        }

        [Route("quote")]
        [HttpPost]
        public async Task<IHttpActionResult> SearchProgressQuote(QuoteProgressSearchModel quoteProgressSearchModel)
        {
            var result = await _searchService.SearchProgressQuote(quoteProgressSearchModel);
            return Ok(new
            {
                data=result
            });
        }


        [Route("currentquote")]
        [HttpPost]
        public async Task<IHttpActionResult> SearchCurrentQuote(QuoteCurrentSearchModel model)
        {
            var result = await _searchService.SearchCurrentQuote(model);
            return Ok(new
            {
                data = result
            });
        }


        [Route("key")]
        [HttpGet]
        public IHttpActionResult SearchSiteByKey(string key = null, int? take = null)
        {
            var results = _searchService.SearchSiteBykey(key, take);
            return Ok(new
            {
                data=results
            });
        }
    }
}
