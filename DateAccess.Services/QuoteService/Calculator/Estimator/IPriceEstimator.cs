using DataAccess.EntityFramework.Models.Quote.Cost;
using DataAccess.EntityFramework.Models.Quote.Cost.Equipment;
using DataAccess.EntityFramework.Models.Quote.Cost.Periodical;
using DataAccess.EntityFramework.Models.Quote.Cost.Supply;
using DateAccess.Services.QuoteService.Calculator.Formula;

namespace DateAccess.Services.QuoteService.Calculator.Estimator
{
    public interface IPriceEstimator
    {
        /// <summary>
        /// formula for calculate equipment price
        /// </summary>
        EquipmentFormula EquipmentFormula { get; set; }

        /// <summary>
        /// formula for calculate periodical price
        /// </summary>
        PeriodicalFormula PeriodicalFormula { get; set; }

        /// <summary>
        /// formula for calculate supply price
        /// </summary>
        SupplyFormula SupplyFormula { get; set; }

        decimal EquipmentPrice(EquipmentSupply supply);
        decimal PeriodicalPrice(Periodical periodical, int weeks);
        decimal SupplyPrice(ToiletrySupply supply);
        decimal Total(Cost cost);
    }
}
