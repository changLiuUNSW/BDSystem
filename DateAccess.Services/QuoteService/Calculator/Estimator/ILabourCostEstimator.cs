using System.Collections.Generic;
using DataAccess.EntityFramework.Models.Quote.Cost.Labour;
using DateAccess.Services.QuoteService.Calculator.Formula.Labour;

namespace DateAccess.Services.QuoteService.Calculator.Estimator
{
    public interface ILabourCostEstimator
    {
        /// <summary>
        /// return wage cost on a single entry
        /// </summary>
        /// <param name="estimation"></param>
        /// <returns></returns>
        decimal Wage(LabourEstimation estimation);

        /// <summary>
        /// return periodical cost on a single periodical job
        /// </summary>
        /// <param name="periodical"></param>
        /// <returns></returns>
        decimal Periodical(LabourPeriodical periodical);

        /// <summary>
        /// return total allowance cost for current labour model
        /// </summary>
        /// <param name="labour"></param>
        /// <returns></returns>
        decimal Allowance(Labour labour);

        /// <summary>
        /// return total cost for current labour model
        /// </summary>
        /// <param name="labour"></param>
        /// <returns></returns>
        decimal Total(Labour labour);

        /// <summary>
        /// formula for calculate allowance cost
        /// </summary>
        AllowanceFormula AllowanceFormula { get; set; }

        /// <summary>
        /// formula for calculate wage cost
        /// </summary>
        WageFormula WageFormula { get; set; }

        /// <summary>
        /// formula for calculate periodical cost
        /// </summary>
        PeriodicalFormula PeriodicalFormula { get; set; }

        /// <summary>
        /// formula for calculate on-cost 
        /// </summary>
        OnCostFormula OnCostFormula { get; set; }
    }
}
