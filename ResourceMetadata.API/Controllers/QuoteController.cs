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
using DataAccess.EntityFramework.TypeLibrary;
using DateAccess.Services.QuoteService;
using Microsoft.AspNet.Identity;

namespace ResourceMetadata.API.Controllers
{
    /// <summary>
    /// quote specific operation
    /// </summary>
    [RoutePrefix("api/quote")]
    [Authorize]
    public class QuoteController : ApiController
    {
        private readonly IQuoteService _quoteService;
        private readonly ApplicationSettings _settings;

        /// <summary>
        ///     default constructor
        /// </summary>
        /// <param name="quoteService"></param>
        /// <param name="settings"></param>
        public QuoteController(IQuoteService quoteService, ApplicationSettings settings)
        {
            _quoteService = quoteService;
            _settings = settings;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            var quote = _quoteService.GetByKey(id);
            if (quote == null) return NotFound();
            return Ok(new
            {
                data=quote
            });
        }

        [Route("cancel")]
        [HttpPost]
        public IHttpActionResult Cancel([FromBody] int id)
        {
            var user = User.Identity.GetUserName();
            var quote = _quoteService.CancelQuote(id, user);
            return Ok(new
            {
                data = quote
            });
        }


        [Route("reviewfailed")]
        [HttpPost]
        public async Task<IHttpActionResult> Reviewfailed([FromBody]ReviewFailedModel model)
        {
            var user = User.Identity.GetUserName();
            var quote = await _quoteService.Reviewfailed(user, model);
            return Ok(new
            {
                data=quote
            });
        }


        [Route("resolve")]
        [HttpPost]
        public async Task<IHttpActionResult> Resolve([FromBody]ResolveModel model)
        {
            var user = User.Identity.GetUserName();
            await _quoteService.Resolved(user,model);
            return Ok();
        }

        [Route("resolveupload")]
        [HttpPost]
        public async Task<IHttpActionResult> ResolveWithUpload()
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
            var model = UploaderHelper.GetFormData<ResolveModel>(result);
            await _quoteService.ResolveWithUpload(user, model, tempFile, originalFileName);
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
            var provider = UploaderHelper.GetMultipartProvider(AppDomain.CurrentDomain.BaseDirectory+@"/"+_settings.TempFolder);
            var result = await Request.Content.ReadAsMultipartAsync(provider);
            var tempFile = new FileInfo(result.FileData.First().LocalFileName);
            var originalFileName = UploaderHelper.GetDeserializedFileName(result.FileData.First());
            var model = UploaderHelper.GetFormData<QuotePostModel>(result);
            var quote = await _quoteService.UploadQuote(model, tempFile, originalFileName, user);
            return Ok(new
            {
                data = quote
            });
        }

        [Route("rate/{id}")]
        [HttpPost]
        public IHttpActionResult UpdateRate(int id,[FromBody] int rate)
        {
           if(!_quoteService.Repository.Any(l => l.Id == id))
            return NotFound();
            var quote = _quoteService.GetByKey(id);
            quote.SuccessRate = rate;
            quote.LastUpdateDate = DateTime.Now;
            _quoteService.Update(quote);
            return Ok(new
            {
                data=quote
            });
        }

        [Route("reupload")]
        [HttpPost]
        public async Task<IHttpActionResult> ReUpload()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return BadRequest("Unsupported Media Type");
            }
            var provider = UploaderHelper.GetMultipartProvider(AppDomain.CurrentDomain.BaseDirectory + @"/" + _settings.TempFolder);
            var result = await Request.Content.ReadAsMultipartAsync(provider);
            var tempFile = new FileInfo(result.FileData.First().LocalFileName);
            var originalFileName = UploaderHelper.GetDeserializedFileName(result.FileData.First());
            var model = UploaderHelper.GetFormData<QuotePostModel>(result);
            var quote = _quoteService.ReUploadQuote(model,tempFile, originalFileName);
            return Ok(new
            {
                data = quote
            });
        }


        [Route("bdreview")]
        [HttpPost]
        public async Task<IHttpActionResult> BdReviewPass(QuotePostModel model)
        {
            string user = User.Identity.GetUserName();
            var quote =await _quoteService.BdReviewPass(model, user, false);
            return Ok(new
            {
                data=quote
            });
        }

        [Route("bdgmreview")]
        [HttpPost]
        public async Task<IHttpActionResult> BdGmReviewPass(QuotePostModel model)
        {
            string user = User.Identity.GetUserName();
            var quote = await _quoteService.BdReviewPass(model, user, true);
            return Ok(new
            {
                data = quote
            });
        }


        [Route("prefinalreview")]
        [HttpPost]
        public async Task<IHttpActionResult> PreFinalReviewPass(QuotePostModel model)
        {
            string user = User.Identity.GetUserName();
            var quote = await _quoteService.PreFinalReviewPass(model, user);
            return Ok(new
            {
                data = quote
            });
        }


        [Route("finalreview")]
        [HttpPost]
        public async Task<IHttpActionResult> FinalReviewPass(QuotePostModel model)
        {
            string user = User.Identity.GetUserName();
            var quote = await _quoteService.FinalReviewPass(model, user);
            return Ok(new
            {
                data = quote
            });
        }


        [Route("qpreview")]
        [HttpPost]
        public async Task<IHttpActionResult> QpReviewPass(QuotePostModel model)
        {
            string user = User.Identity.GetUserName();
            var quote = await _quoteService.QpReviewPass(model, user);
            return Ok(new
            {
                data = quote
            });
        }

        [Route("print")]
        [HttpPost]
        public async Task<IHttpActionResult> Print(QuotePostModel model)
        {
            string user = User.Identity.GetUserName();
            var quote =  await _quoteService.Print(model, user);
            return Ok(new
            {
                data = quote
            });
        }

        [Route("pricecheckfailed")]
        [HttpPost]
        public async Task<IHttpActionResult> PriceCheckfailed([FromBody]ReviewFailedModel model)
        {
            var user = User.Identity.GetUserName();
            var quote = await _quoteService.PriceCheckfailed(user, model);
            return Ok(new
            {
                data = quote
            });
        }

        [Route("status")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAllStatus()
        {
            var statusList = await _quoteService.GetAllStatus();
            return Ok(new
            {
                data = statusList
            });
        }

        [Route("overdue")]
        [HttpGet]
        public async Task<IHttpActionResult> GetOverDueList()
        {
            var overdueList = await _quoteService.GetOverDueList();
            return Ok(new
            {
                data = overdueList
            });
        }

        [Route("send")]
        [HttpPost]
        public async Task<IHttpActionResult> SendToWp([FromBody]QuotePostModel model)
        {
            string user = User.Identity.GetUserName();
            var quote = await _quoteService.SendToWp(model, user);
            return Ok(new
            {
                data=quote
            });
        }

        [Route("question")]
        [HttpGet]
        public IHttpActionResult GetQuestions(int questionType)
        {
            var list = _quoteService.GetQuestionsByType(questionType);
            return Ok(new
            {
                data = list
            });
        }


        [Route("finalize/{id}")]
        [HttpPost]
        public IHttpActionResult Finalize(int id,List<QuestionResultModel> models)
        {
            string user = User.Identity.GetUserName();
            var quote = _quoteService.Finalize(user,id, models);
            return Ok(new
            {
                data = quote
            });
        }


        [Route("download/{id}")]
        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage Download(int id)
        {
            var quote = _quoteService.GetByKey(id);
            if (quote == null) return new HttpResponseMessage(HttpStatusCode.NotFound);
            var root = AppDomain.CurrentDomain.BaseDirectory + _settings.QuoteUploadPath + @"/" + quote.Id;
            if (string.IsNullOrEmpty(quote.FileName)) return new HttpResponseMessage(HttpStatusCode.NotFound);
            var path=Path.Combine(root, quote.FileName);
            if (!File.Exists(path))
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }
            var result = new HttpResponseMessage(HttpStatusCode.OK);
            var stream = new FileStream(path, FileMode.Open);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = quote.FileName
            };
            result.Content.Headers.ContentType = new MediaTypeHeaderValue(MediaTypeNames.Application.Octet);
            return result;
        }

        [Route("pricepage/{id}")]
        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage DownloadPricePage(int id)
        {
            var quote = _quoteService.GetByKey(id);
            if (quote == null) return new HttpResponseMessage(HttpStatusCode.NotFound);
            var root = AppDomain.CurrentDomain.BaseDirectory + _settings.QuoteUploadPath + @"/" + quote.Id;
            if (string.IsNullOrEmpty(quote.PricePageName)) return new HttpResponseMessage(HttpStatusCode.NotFound);
            var path = Path.Combine(root, quote.PricePageName);
            if (!File.Exists(path))
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }
            var result = new HttpResponseMessage(HttpStatusCode.OK);
            var stream = new FileStream(path, FileMode.Open);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = quote.PricePageName
            };
            result.Content.Headers.ContentType = new MediaTypeHeaderValue(MediaTypeNames.Application.Octet);
            return result;
        }


        [Route("contact")]
        [HttpPost]
        public IHttpActionResult Contact(QuotePostModel model)
        {
            var quote = _quoteService.Contact(model);
            return Ok(new
            {
                data = quote
            });
        }


        [Route("dead/{id}")]
        [HttpPost]
        public IHttpActionResult Dead(int id, List<QuestionResultModel> models)
        {
            string user = User.Identity.GetUserName();
            var quote = _quoteService.Dead(user,id,models);
            return Ok(new
            {
                data=quote
            });
        }

        [Route("notdead/{id}")]
        [HttpPost]
        public IHttpActionResult NotDead(int id, List<QuestionResultModel> models)
        {
            var user = User.Identity.GetUserName();
            var quote = _quoteService.NotDead(user, id, models);
            return Ok(new
            {
                data = quote
            });
        }

        [Route("notcontact/{id}")]
        [HttpPost]
        public IHttpActionResult NotContact(int id, List<QuestionResultModel> models)
        {
            string user = User.Identity.GetUserName();
            var quote = _quoteService.NotContact(user,id, models);
            return Ok(new
            {
                data = quote
            });
        }

        [Route("notsendemail/{id}")]
        [HttpPost]
        public IHttpActionResult NotSendEmail(int id, List<QuestionResultModel> models)
        {
            string user = User.Identity.GetUserName();
            var quote = _quoteService.NotSendEmail(user, id, models);
            return Ok(new
            {
                data = quote
            });
        }

        [Route("sendemail/{id}")]
        [HttpPost]
        public IHttpActionResult SendEmail(int id)
        {
            var quote = _quoteService.GetByKey(id);
            if (quote == null) return NotFound();
            quote.ClientEmailSendReminderDisabled = true;
            _quoteService.Update(quote);
            return Ok(new
            {
                data = quote
            });
        }


        [Route("notadjust/{id}")]
        [HttpPost]
        public IHttpActionResult NotAdjust(int id, List<QuestionResultModel> models)
        {
            string user = User.Identity.GetUserName();
            var quote = _quoteService.NotAdjust(user, id, models);
            return Ok(new
            {
                data = quote
            });
        }

        [Route("adjust")]
        [HttpPost]
        public async Task<IHttpActionResult> Adjust([FromBody]ReviewFailedModel model)
        {
            var user = User.Identity.GetUserName();
            var quote = await _quoteService.Adjust(user, model);
            return Ok(new
            {
                data = quote
            });
        }

        [Route("result/{id}")]
        [HttpGet]
        public IHttpActionResult GetQuestionResultsByType(int id, int? type)
        {
            var questionType = type == null ? (QuoteQuestionType?) null : (QuoteQuestionType) type;
            var result = _quoteService.GetQuoteResultListByType(id, questionType);
            return Ok(new
            {
                data = result
            });
        }
    }
}
