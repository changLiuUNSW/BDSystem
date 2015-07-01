using System.Linq;
using DataAccess.EntityFramework.Models.Quote.Cost;
using DataAccess.EntityFramework.Models.Quote.Cost.Equipment;
using DataAccess.EntityFramework.Models.Quote.Cost.Periodical;
using DataAccess.EntityFramework.Models.Quote.Cost.Supply;
using DateAccess.Services.QuoteService.Calculator.Formula;

namespace DateAccess.Services.QuoteService.Calculator.Estimator
{
    internal class PriceEstimator : IPriceEstimator
    {
        public PriceEstimator()
        {
            EquipmentFormula = new EquipmentFormula();
            PeriodicalFormula = new PeriodicalFormula();
            SupplyFormula = new SupplyFormula();
        }

        public EquipmentFormula EquipmentFormula { get; set; }
        public PeriodicalFormula PeriodicalFormula { get; set; }
        public SupplyFormula SupplyFormula { get; set; }

        /// <summary>
        /// return equipment price per year
        /// </summary>
        /// <param name="supply"></param>
        /// <returns></returns>
        public decimal EquipmentPrice(EquipmentSupply supply)
        {
            supply.Total = EquipmentFormula.PricePw(
                supply.Equipment.Cost,
                supply.Equipment.Machine.YearsAllocated,
                (decimal) supply.Equipment.Machine.RepairFactor,
                supply.Equipment.Machine.Fuels);

            return supply.Total.GetValueOrDefault();
        }

        /// <summary>
        /// return single periodical work price per week
        /// </summary>
        /// <param name="periodical"></param>
        /// <param name="weeks"></param>
        /// <returns></returns>
        public decimal PeriodicalPrice(Periodical periodical, int weeks)
        {
            return PeriodicalFormula.PricePa(
                periodical.CostPerTime.GetValueOrDefault(),
                periodical.FreqPa.GetValueOrDefault()) / weeks;
        }

        /// <summary>
        /// return supply price per week
        /// </summary>
        /// <param name="supply"></param>
        /// <returns></returns> 
        public decimal SupplyPrice(ToiletrySupply supply)
        {
            supply.Total = SupplyFormula.PricePw(supply.ToiletRequisite.Price, supply.UnitsPw.GetValueOrDefault());
            return supply.Total.GetValueOrDefault();
        }

        /// <summary>
        /// return total price for the cost
        /// </summary>
        /// <param name="cost"></param>
        /// <returns></returns>
        public decimal Total(Cost cost)
        {
            if (cost.EquipmentSupplies != null)
                cost.RegEquipmentCostPw = cost.EquipmentSupplies.Sum(x => EquipmentPrice(x));

            if (cost.Periodicals != null)
            {
                cost.RegPeriodicalCostPw =
                    cost.Periodicals.Sum(x => PeriodicalPrice(x, cost.RegCleanWeeks.GetValueOrDefault()));
            }

            if (cost.ToiletrySupplies != null) 
                cost.RegSupplyCostPw = cost.ToiletrySupplies.Sum(x => SupplyPrice(x));

            cost.SubTotal = cost.RegSubcontractorCostPw.GetValueOrDefault() +
                            cost.RegPeriodicalCostPw.GetValueOrDefault() +
                            cost.RegSupplyCostPw.GetValueOrDefault() +
                            cost.RegEquipmentCostPw.GetValueOrDefault();

            cost.Total = cost.SubTotal.GetValueOrDefault() +
                         cost.Return.GetValueOrDefault() +
                         cost.Liability.GetValueOrDefault();

            return cost.Total.GetValueOrDefault();
        }
    }
}
