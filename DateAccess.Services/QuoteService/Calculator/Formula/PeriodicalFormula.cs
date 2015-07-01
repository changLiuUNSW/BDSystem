namespace DateAccess.Services.QuoteService.Calculator.Formula
{
    public class PeriodicalFormula
    {
        public virtual decimal PricePa(decimal cost, decimal freq)
        {
            return cost * freq;
        }
    }
}
