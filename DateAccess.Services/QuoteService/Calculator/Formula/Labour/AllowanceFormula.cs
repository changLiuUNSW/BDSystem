using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DateAccess.Services.QuoteService.Calculator.Formula.Labour
{
    public class AllowanceFormula
    {
        public virtual decimal PerWeek(decimal rate, decimal num)
        {
            return rate * num;
        }
    }
}
