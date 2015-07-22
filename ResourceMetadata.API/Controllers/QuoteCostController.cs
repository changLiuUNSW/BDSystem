using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using DataAccess.Common;
using DataAccess.Common.Util;
using DataAccess.EntityFramework.Models.Quote.Cost;
using DataAccess.EntityFramework.Models.Quote.Cost.Labour;
using DataAccess.EntityFramework.Models.Quote.Specification;
using DateAccess.Services.QuoteService;
using Microsoft.AspNet.Identity;

namespace ResourceMetadata.API.Controllers
{
    [RoutePrefix("api/Cost")]
    //[Authorize]
    public class QuoteCostController : ApiController
    {
        private readonly IQuoteCostService _quoteCostService;
        private readonly ApplicationSettings _settings;

        public QuoteCostController(IQuoteCostService quoteCostService, ApplicationSettings settings)
        {
            _quoteCostService = quoteCostService;
            _settings = settings;
        }

        [Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            var lead = _quoteCostService.GetByKey(id);
            if (lead == null) return NotFound();
            return Ok(new
            {
                data = lead
            }
            );
        }

        [Route("finalize")]
        [HttpPost]
        public IHttpActionResult Finalize(List<int> costIds)
        {
            _quoteCostService.Finalize(costIds);
            return Ok();
        }

        [Route("bulkdelete")]
        [HttpPost]
        public IHttpActionResult DeleteQuoteCosts(List<int> costIds)
        {
            _quoteCostService.Delete(costIds);
            return Ok();
        }


        [Route("upload")]
        [HttpPost]
        public async Task<IHttpActionResult> Upload()
        {
            string user = User.Identity.GetUserName();
            if (!Request.Content.IsMimeMultipartContent())
            {
                return BadRequest("Unsupported Media Type");
            }
            var provider =
                UploaderHelper.GetMultipartProvider(AppDomain.CurrentDomain.BaseDirectory + @"/" + _settings.TempFolder);
            var result = await Request.Content.ReadAsMultipartAsync(provider);
            var tempFile = new FileInfo(result.FileData.First().LocalFileName);
            var originalFileName = UploaderHelper.GetDeserializedFileName(result.FileData.First());
            var costModel = UploaderHelper.GetFormData<Cost>(result);
            var filePath = AppDomain.CurrentDomain.BaseDirectory + _settings.CostUploadPath + @"/" + costModel.QuoteId;
            if (!Directory.Exists(filePath)) Directory.CreateDirectory(filePath);
            Cost cost = costModel.Id <= 0
                ? _quoteCostService.CreateUploadCost(costModel, tempFile, originalFileName, filePath, user)
                : _quoteCostService.UpdateUploadCost(costModel, tempFile, originalFileName, filePath);
            return Ok(new
            {
                data = cost
            });
        }

        [Route("download/security")]
        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage DownloadSecurity()
        {
            const string filename = "Security Estimation workbook.xlsm";
            string path = AppDomain.CurrentDomain.BaseDirectory + _settings.SecurityWorkBookPath;
            if (!File.Exists(path))
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }
            var result = new HttpResponseMessage(HttpStatusCode.OK);
            var stream = new FileStream(path, FileMode.Open);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = filename
            };
            result.Content.Headers.ContentType = new MediaTypeHeaderValue(MediaTypeNames.Application.Octet);
            return result;
        }

        [Route("download/nz")]
        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage DownloadNZ()
        {
            const string filename = "NZ small-medium Estimation workbook.xlsm";
            string path = AppDomain.CurrentDomain.BaseDirectory + _settings.NzWorkBookPath;
            if (!File.Exists(path))
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }
            var result = new HttpResponseMessage(HttpStatusCode.OK);
            var stream = new FileStream(path, FileMode.Open);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = filename
            };
            result.Content.Headers.ContentType = new MediaTypeHeaderValue(MediaTypeNames.Application.Octet);
            return result;
        }

        [Route("create")]
        [HttpPost]
        public IHttpActionResult Create(Cost model)
        {
            var user = User.Identity.GetUserName();
            var cost = _quoteCostService.CreateSystemCost(model, user);
            return Ok(new
            {
                data = cost
            });
        }

        [Route("equipment")]
        public IHttpActionResult GetEquipment(string prefix = null)
        {
            if (prefix != null)
            {
                return Ok(new
                {
                    data = _quoteCostService.UnitOfWork.EquipmentRepository.Get(x => x.EquipmentCode.StartsWith(prefix))
                });
            }

            return Ok(new
            {
                data = _quoteCostService.UnitOfWork.EquipmentRepository.Get()
            });
        }


        [Route("Toiletry")]
        public IHttpActionResult GetToiletry(string prefix = null)
        {
            if (prefix != null)
            {
                return Ok(new
                {
                    data =
                        _quoteCostService.UnitOfWork.ToiletRequisiteRepository.Get(x => x.ItemCode.StartsWith(prefix))
                });
            }

            return Ok(new
            {
                data = _quoteCostService.UnitOfWork.ToiletRequisiteRepository.Get()
            });
        }


        [Route("qoutesource")]
        public IHttpActionResult GetQuoteSource(string keyword=null)
        {
            var quoteSourceRepo = _quoteCostService.UnitOfWork.GetRepository<QuoteSource>();
            if (!string.IsNullOrEmpty(keyword))
            {
                return Ok(new
                {
                    data =quoteSourceRepo.Get(l => l.Description.Contains(keyword))
                });
            }
            return Ok(new
            {
                data = quoteSourceRepo.Get()
            });
        }

        [Route("industrytype")]
        public IHttpActionResult GetIndustryType(string keyword = null,int? top=null)
        {
            var industryTypeRepo = _quoteCostService.UnitOfWork.GetRepository<CleaningSpec>();
            var result = !string.IsNullOrEmpty(keyword) ? 
                industryTypeRepo.GetTop(top, l => l.Name.Contains(keyword)) : industryTypeRepo.GetTop(top);
            return Ok(new
            {
                data = result
            });
        }


        [Route("liability")]
        public IHttpActionResult GetPublicLiability()
        {
            var result = _quoteCostService.UnitOfWork.GetRepository<PublicLiability>().Get();
            return Ok(new
            {
                data = result
            });
        }

        [Route("standardregion")]
        public IHttpActionResult GetStandardRegion()
        {
            var result = _quoteCostService.UnitOfWork.GetRepository<StandardRegion>().Get();
            return Ok(new
            {
                data=result
            });
        }





        /// <summary>
        /// return full labour rate list
        /// </summary>
        /// <returns></returns>
        [Route("rate/labour")]
        public IHttpActionResult GetLabourRate()
        {
            return Ok(new
            {
                data = _quoteCostService.UnitOfWork.GetRepository<LabourRate>().Get()
            });
        }


//        /// <summary>
//        /// return requested cost model
//        /// </summary>
//        /// <param name="id"></param>
//        /// <returns></returns>
//        [Route("{id}")]
//        public IHttpActionResult GetCost(int id)
//        {
//            return Ok(new
//            {
//                data = _quoteCostService.GetByKey(id)
//            });
//        }


//
//        /// <summary>
//        /// return full allowance rate list
//        /// </summary>
//        /// <returns></returns>
//        [Route("rate/allowance")]
//        public IHttpActionResult GetAllowanceRate()
//        {
//            return Ok(new
//            {
//                data = _quoteCostService.UnitOfWork.GetRepository<AllowanceRate>().Get()
//            });
//        }
//
//        /// <summary>
//        /// return full onCost rate list
//        /// </summary>
//        /// <returns></returns>
//        [Route("rate/onCost")]
//        public IHttpActionResult GetOnCostRate()
//        {
//            return Ok(new
//            {
//                data = _quoteCostService.UnitOfWork.GetRepository<OnCostRate>().Get()
//            });
//        }
    }
}