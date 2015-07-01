using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DataAccess.EntityFramework.Models.Quote.Cost.Labour
{
    [Table("Labour", Schema = "Quote")]
    public class Labour
    {
        [Key, ForeignKey("Cost")]
        public int CostId { get; set; }
        public virtual Cost Cost { get; set; }

        [DefaultValue(52)]
        public int? WeeksToInvoice { get; set; }
        [DefaultValue(100)]
        public int OnCostFactor { get; set; }


        public virtual ICollection<LabourEstimation> LabourEstimations { get; set; }
        public virtual ICollection<LabourPeriodical> LabourPeriodicals { get; set; }
            
        [ForeignKey("AllowanceRate")]
        public int? AllowanceRateId { get; set; }
        public virtual AllowanceRate AllowanceRate { get; set; }

        [ForeignKey("OnCostRate")]
        public int? OnCostRateId { get; set; }
        public virtual OnCostRate OnCostRate { get; set; }

        public int? ToiletAllowance { get; set; }
        public int? LeadingHandSmall { get; set; }
        public int? LeadingHandLarge { get; set; }
        public int? OtherAllowance { get; set; }

        public decimal? OtherAllowanceRate { get; set; }

        //allowance Cost
        public decimal? ToiletAllowancePw { get; set; }
        public decimal? LeadingHandSmallPw { get; set; }
        public decimal? LeadingHandLargePw { get; set; }
        public decimal? OtherAllowancePw { get; set; }
        public decimal? LeapYearPw { get; set; }
        public decimal? PicnicDayPw { get; set; }

        //pay
        public decimal? HolidayPayPw { get; set; }
        public decimal? SickPayPw { get; set; }
        public decimal? LongServicesLeavePw { get; set; }
        public decimal? WorkerCompensationPw { get; set; }
        public decimal? SuperannuationPw { get; set; }
        public decimal? PayrollTaxPw { get; set; }
        public decimal? IncomeProtectionPw { get; set; }

        //material Cost
        public decimal? MaterialCost { get; set; }
        public decimal? Uniforms { get; set; }
        public int? UniformsRequired { get; set; }
        public decimal? CriminalCheck { get; set; }
        public int? CriminalCheckRequired { get; set; }
        public decimal? Phone { get; set; }
        public int? PhoneRequired { get; set; }
        public decimal? OtherCost { get; set; }
    }
}
