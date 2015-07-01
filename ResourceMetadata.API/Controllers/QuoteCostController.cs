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
using DataAccess.Common;
using DataAccess.Common.Util;
using DataAccess.EntityFramework;
using DataAccess.EntityFramework.Models.Quote.Cost;
using DateAccess.Services.QuoteService;
using Microsoft.AspNet.Identity;

namespace ResourceMetadata.API.Controllers
{
    [RoutePrefix("api/Cost")]
    [Authorize]
    public class QuoteCostController : ApiController
    {
        private readonly IQuoteCostService _quoteCostService;
        private readonly ApplicationSettings _settings;
        private readonly IUnitOfWork _unitOfWork;

        public QuoteCostController(IQuoteCostService quoteCostService, ApplicationSettings settings, IUnitOfWork unitOfWork)
        {
            _quoteCostService = quoteCostService;
            _settings = settings;
            _unitOfWork = unitOfWork;
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
            var provider = UploaderHelper.GetMultipartProvider(AppDomain.CurrentDomain.BaseDirectory + @"/" + _settings.TempFolder);
            var result = await Request.Content.ReadAsMultipartAsync(provider);
            var tempFile = new FileInfo(result.FileData.First().LocalFileName);
            var originalFileName = UploaderHelper.GetDeserializedFileName(result.FileData.First());
            var costModel = UploaderHelper.GetFormData<Cost>(result);
            var filePath = AppDomain.CurrentDomain.BaseDirectory + _settings.CostUploadPath + @"/"+costModel.QuoteId;
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

        /// <summary>
        /// get all available equipments for cost estimation
        /// </summary>
        /// <returns></returns>
        [Route("equipment")]
        public IHttpActionResult GetEquipment()
        {
            return Ok(new
            {
                data = _unitOfWork.EquipmentRepository.Get()
            });
        }

        /// <summary>
        /// for test only
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{id}")]
        public IHttpActionResult GetCost(int id)
        {
            return Ok(new
            {
                data = new Cost()
            });
        }
    }
}