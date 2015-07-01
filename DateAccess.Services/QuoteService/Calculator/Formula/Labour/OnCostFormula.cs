using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DateAccess.Services.QuoteService.Calculator.Formula.Labour
{
    public class OnCostFormula
    {
        public virtual decimal Cost(decimal rate, decimal sum)
        {
            return rate * sum;
        }
    }
}
