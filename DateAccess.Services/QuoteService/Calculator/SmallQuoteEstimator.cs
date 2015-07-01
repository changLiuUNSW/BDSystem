using DataAccess.EntityFramework.Models.Quote.Cost;
using DateAccess.Services.QuoteService.Calculator.Estimator;

namespace DateAccess.Services.QuoteService.Calculator
{
    public abstract class SmallQuoteEstimator
    {
        private SmallQuoteEstimator()
        {
        }

        private sealed class AusEstimator : SmallQuoteEstimator
        {
            static AusEstimator() { }
            internal static readonly SmallQuoteEstimator Instance = new AusEstimator();

            private AusEstimator()
            {
                LabourEstimator = new LabourCostEstimator();
                PriceEstimator = new PriceEstimator();
            }
        };

        private sealed class NzEstimator : SmallQuoteEstimator
        {
            static NzEstimator() { }
            internal static readonly  SmallQuoteEstimator Instance = new NzEstimator();

            private NzEstimator()
            {
                //todo nz uses the same formula for now
                LabourEstimator = new LabourCostEstimator();
                PriceEstimator = new PriceEstimator();
            }
        };

        public virtual ILabourCostEstimator LabourEstimator { get; set; }
        public virtual IPriceEstimator PriceEstimator { get; set; }

        public static SmallQuoteEstimator Australia
        {
            get { return AusEstimator.Instance; }
        }

        public static SmallQuoteEstimator NewZealand
        {
            get { return NzEstimator.Instance; }
        }

        /// <summary>
        /// return total cost base on the cost model
        /// </summary>
        /// <param name="cost"></param>
        /// <returns></returns>
        public virtual Cost GetCost(Cost cost)
        {
            if (cost == null)
                return null;

            LabourEstimator.Total(cost.Labour);
            PriceEstimator.Total(cost);

            return cost;
        }
    }
}
