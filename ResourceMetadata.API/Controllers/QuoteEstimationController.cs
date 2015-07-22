using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using DataAccess.EntityFramework.Models.Quote.Cost.Equipment;
using DataAccess.EntityFramework.Models.Quote.Cost.Labour;
using DataAccess.EntityFramework.Models.Quote.Cost.Periodical;
using DataAccess.EntityFramework.Models.Quote.Cost.Supply;
using DateAccess.Services.QuoteService.Calculator;

namespace ResourceMetadata.API.Controllers
{
    /// <summary>
    /// cost estimation calculator
    /// </summary>
    [RoutePrefix("api/quote/calculator")]
    public class QuoteEstimationController : ApiController
    {
        /// <summary>
        /// calculate labour wage and return the original object with updated cost
        /// </summary>
        /// <param name="labours"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("labourWage")]
        public IHttpActionResult Wage(List<LabourEstimation> labours)
        {
            labours.AsParallel().ForAll(x=>SmallQuoteEstimator.Australia.LabourEstimator.Wage(x));

            return Ok(new
            {
                data = labours
            });
        }

        /// <summary>
        /// calculate labour periodical and return the original object with updated cost
        /// </summary>
        /// <param name="periodicals"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("labourPeriodical")]
        public IHttpActionResult Periodical(List<LabourPeriodical> periodicals)
        {
            periodicals.AsParallel().ForAll(x=>SmallQuoteEstimator.Australia.LabourEstimator.Periodical(x));
            return Ok(new
            {
                data = periodicals
            });
        }


        /// <summary>
        /// calculate labour allowance and return the original object with updated cost
        /// </summary>
        /// <param name="labour"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("labourAllowance")]
        public IHttpActionResult Allowance(Labour labour)
        {
            SmallQuoteEstimator.Australia.LabourEstimator.Allowance(labour);
            return Ok(new
            {
                data = labour
            });
        }

        /// <summary>
        /// calculate the entire labour model and return the updated model
        /// </summary>
        /// <param name="labour"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("labourTotal")]
        public IHttpActionResult Total(Labour labour)
        {
            SmallQuoteEstimator.Australia.LabourEstimator.Total(labour);
            return Ok(new
            {
                data = labour
            });
        }

        /// <summary>
        /// calculate the equipment cost
        /// </summary>
        [HttpPost]
        [Route("equipment")]
        public IHttpActionResult Equipment(List<EquipmentSupply> equipments)
        {
            equipments.AsParallel()
                .ForAll(equipment => SmallQuoteEstimator.Australia.PriceEstimator.EquipmentPrice(equipment));

            return Ok(new
            {
                data = equipments
            });
        }

        /// <summary>
        /// calculate toiletry supplies cost
        /// </summary>
        /// <param name="supplies"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("supply")]
        public IHttpActionResult Supply(List<ToiletrySupply> supplies)
        {
            supplies.AsParallel().ForAll(supply => SmallQuoteEstimator.Australia.PriceEstimator.SupplyPrice(supply));

            return Ok(new
            {
                data = supplies
            });
        }

        /// <summary>
        /// calculate periodical costs
        /// </summary>
        /// <param name="periodicals"></param>
        /// <param name="weeks"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("periodical")]
        public IHttpActionResult Periodical(List<Periodical> periodicals, int weeks)
        {
            periodicals.AsParallel()
                .ForAll(periodical => SmallQuoteEstimator.Australia.PriceEstimator.PeriodicalPrice(periodical, weeks));

            return Ok(new
            {
                data = periodicals
            });
        }
    }
}
