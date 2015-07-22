using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DateAccess.Services.QuoteService.Calculator.Formula.Labour
{
    public class WageFormula
    {
        public virtual decimal Weekday(decimal mins, decimal rate, decimal days)
        {
            return mins / 60 * rate * days;
        }

        public virtual decimal Weekend(decimal mins, decimal rate)
        {
            return rate / 60 * mins;
        }

        //TODO incorrect formula on the excel
        public virtual decimal Holiday(decimal mins, decimal rate, decimal holidayFactor, decimal weeksToInvoice)
        {
            return 0;
        }
    }
}
