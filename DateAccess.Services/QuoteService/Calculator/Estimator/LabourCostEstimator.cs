using System.Linq;
using DataAccess.EntityFramework.Models.Quote.Cost.Labour;
using DateAccess.Services.QuoteService.Calculator.Formula.Labour;

namespace DateAccess.Services.QuoteService.Calculator.Estimator
{
    internal class LabourCostEstimator : ILabourCostEstimator
    {
        public LabourCostEstimator()
        {
            AllowanceFormula = new AllowanceFormula();
            WageFormula = new WageFormula();
            PeriodicalFormula = new PeriodicalFormula();
            OnCostFormula = new OnCostFormula();
        }

        public LabourCostEstimator(AllowanceFormula allowanceFormula, WageFormula wageFormula, PeriodicalFormula periodicalFormula, OnCostFormula onCostFormula)
        {
            AllowanceFormula = allowanceFormula;
            WageFormula = wageFormula;
            PeriodicalFormula = periodicalFormula;
            OnCostFormula = onCostFormula;
        }

        public AllowanceFormula AllowanceFormula { get; set; }
        public WageFormula WageFormula { get; set; }
        public PeriodicalFormula PeriodicalFormula { get; set; }
        public OnCostFormula OnCostFormula { get; set; }

        /// <summary>
        /// return sum of weekday, saturday, sunday and holiday wage cost
        /// </summary>
        /// <param name="estimation"></param>
        /// <returns></returns>
        public decimal Wage(LabourEstimation estimation)
        {
            estimation.WeekdayTotal = WageFormula.Weekday(estimation.MinsOnWeekdays.GetValueOrDefault(),
                                              estimation.LabourRate.Weekdays,
                                              estimation.DaysPerWeek.GetValueOrDefault());

            estimation.SaturdayTotal = WageFormula.Weekend(estimation.MinsOnSat.GetValueOrDefault(),
                                                           estimation.LabourRate.Saturday);

            estimation.SundayTotal = WageFormula.Weekend(estimation.MinsOnSun.GetValueOrDefault(),
                                                         estimation.LabourRate.Sunday);

            estimation.HolidayTotal = WageFormula.Holiday(estimation.MinsOnHoliday.GetValueOrDefault(),
                                                          estimation.LabourRate.Holiday,
                                                          estimation.HolidayFactor,
                                                          estimation.Labour.WeeksToInvoice.GetValueOrDefault());

            estimation.Total = estimation.WeekdayTotal + estimation.SaturdayTotal + estimation.SundayTotal;
            return estimation.Total.GetValueOrDefault();
        }

        /// <summary>
        /// return periodical cost per week
        /// </summary>
        /// <param name="periodical"></param>
        /// <returns></returns>
        public decimal Periodical(LabourPeriodical periodical)
        {
            periodical.Wage = PeriodicalFormula.PerWeek(periodical.Hours.GetValueOrDefault(),
                                            periodical.LabourRate.Weekdays,
                                            periodical.Frequency.GetValueOrDefault(),
                                            periodical.WeeksToInvoice.GetValueOrDefault());

            return periodical.Wage.GetValueOrDefault();
        }

        /// <summary>
        /// return allowance cost per week
        /// </summary>
        /// <param name="labour"></param>
        /// <returns></returns>
        public decimal Allowance(Labour labour)
        {
            labour.ToiletAllowancePw = AllowanceFormula.PerWeek(labour.ToiletAllowance.GetValueOrDefault(),
                                                                labour.AllowanceRate.ToiletAllowPerShift);

            labour.LeadingHandSmallPw = AllowanceFormula.PerWeek(labour.LeadingHandSmall.GetValueOrDefault(),
                                                                 labour.AllowanceRate.LeadingHandSmallGroup);

            labour.LeadingHandLargePw = AllowanceFormula.PerWeek(labour.LeadingHandLarge.GetValueOrDefault(),
                                                                 labour.AllowanceRate.LeadingHandLargeGroup);

            labour.OtherAllowancePw = AllowanceFormula.PerWeek(labour.OtherAllowance.GetValueOrDefault(),
                                                               labour.OtherAllowanceRate.GetValueOrDefault());

            return labour.ToiletAllowancePw.GetValueOrDefault() +
                   labour.LeadingHandSmallPw.GetValueOrDefault() +
                   labour.LeadingHandLargePw.GetValueOrDefault() +
                   labour.OtherAllowancePw.GetValueOrDefault() +
                   labour.LeapYearPw.GetValueOrDefault() +
                   labour.PicnicDayPw.GetValueOrDefault();
        }

        /// <summary>
        /// return total labour cost including wage, periodical, allowance as well as other expenses such as superannuation, compensation, long services and tax etc.
        /// </summary>
        /// <param name="labour"></param>
        /// <returns></returns>
        public virtual decimal Total(Labour labour)
        {
            if (labour == null)
                return 0;

            decimal labourTotal = 0;
            decimal labourTotalWithoutHoliday = 0;
            if (labour.LabourEstimations != null)
            {
                
                labourTotal = labour.LabourEstimations.Sum(x => Wage(x));
                labourTotalWithoutHoliday = labour.LabourEstimations.Sum(x => x.WeekdayTotal.GetValueOrDefault() +
                                                                                  x.SaturdayTotal.GetValueOrDefault() +
                                                                                  x.SundayTotal.GetValueOrDefault());
            }

            decimal periodicalTotal = 0;
            if (labour.LabourPeriodicals != null) 
                periodicalTotal = labour.LabourPeriodicals.Sum(x => Periodical(x));

            var allowance = Allowance(labour);

            var subTotalWithoutHoliday = labourTotalWithoutHoliday +
                                         periodicalTotal +
                                         allowance;

            labour.SickPayPw = OnCostFormula.Cost(labour.OnCostRate.HolidayPay, subTotalWithoutHoliday);
            labour.HolidayPayPw = OnCostFormula.Cost(labour.OnCostRate.SickPay, subTotalWithoutHoliday);

            var subTotal = labourTotal +
                           periodicalTotal +
                           allowance +
                           labour.SickPayPw.GetValueOrDefault() +
                           labour.HolidayPayPw.GetValueOrDefault();

            labour.LongServicesLeavePw = subTotal * labour.OnCostRate.LongService;

            labour.SuperannuationPw = subTotal * labour.OnCostRate.Superannuation;

            labour.WorkerCompensationPw = (subTotal + labour.SuperannuationPw.GetValueOrDefault()) *
                                          labour.OnCostRate.WorkerCompensation;

            labour.PayrollTaxPw = (subTotal + labour.SuperannuationPw.GetValueOrDefault()) * labour.OnCostRate.PayrollTax;

            return subTotal +
                   labour.LongServicesLeavePw.GetValueOrDefault() +
                   labour.SuperannuationPw.GetValueOrDefault() +
                   labour.WorkerCompensationPw.GetValueOrDefault() +
                   labour.PayrollTaxPw.GetValueOrDefault();
        }
    }
}
