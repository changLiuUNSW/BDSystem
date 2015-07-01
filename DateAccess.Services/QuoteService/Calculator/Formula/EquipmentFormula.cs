namespace DateAccess.Services.QuoteService.Calculator.Formula
{
    public class EquipmentFormula
    {
        public virtual decimal PricePw(decimal machineCost, decimal yearsAllocated, decimal repairFactor, decimal fuelCost)
        {
            return CapitalAllocation(machineCost, yearsAllocated) + RepairAllocation(machineCost, repairFactor) + fuelCost;
        }

        public virtual decimal CapitalAllocation(decimal machineCost, decimal yearsAllocated)
        {
            return machineCost / yearsAllocated / 52;
        }

        public virtual decimal RepairAllocation(decimal machineCost, decimal repairFactor)
        {
            return machineCost * repairFactor;
        }
    }
}
