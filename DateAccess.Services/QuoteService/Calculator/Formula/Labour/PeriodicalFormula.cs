using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DateAccess.Services.QuoteService.Calculator.Formula.Labour
{
    public class PeriodicalFormula
    {
        public virtual decimal Base(decimal hours, decimal rate)
        {
            return hours * rate;
        }

        public virtual decimal PerYear(decimal hours, decimal rate, int freq)
        {
            return Base(hours, rate) * freq;
        }

        public virtual decimal PerWeek(decimal hours, decimal rate, int freq, int weeks)
        {
            return PerYear(hours, rate, freq) / weeks;
        }
    }
}
