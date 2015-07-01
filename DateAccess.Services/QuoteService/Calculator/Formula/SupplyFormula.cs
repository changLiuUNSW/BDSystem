namespace DateAccess.Services.QuoteService.Calculator.Formula
{
    public class SupplyFormula
    {
        public virtual decimal PricePw(decimal price, decimal units)
        {
            return price * units;
        }
    }
}
