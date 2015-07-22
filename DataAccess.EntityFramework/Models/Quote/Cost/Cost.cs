using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DataAccess.EntityFramework.Models.BD;
using DataAccess.EntityFramework.Models.Quote.Cost.Equipment;
using DataAccess.EntityFramework.Models.Quote.Cost.Periodical;
using DataAccess.EntityFramework.Models.Quote.Cost.Supply;
using DataAccess.EntityFramework.Models.Quote.Specification;
using DataAccess.EntityFramework.TypeLibrary;

namespace DataAccess.EntityFramework.Models.Quote.Cost
{
    [Table("Cost", Schema = "Quote")]
    public class Cost : QuoteBase
    {

        public Cost()
        {
            DayOfClean=new DayOfClean();
            Address=new Address();
        }
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("Quote")]
        public int QuoteId { get; set; }

        public virtual Quote Quote { get; set; }

        [ForeignKey("Labour")]
        public int? LabourId { get; set; }
        public virtual Labour.Labour Labour { get; set; }

        //equipments
        public virtual ICollection<EquipmentSupply> EquipmentSupplies { get; set; }

        //supplies
        public virtual ICollection<ToiletrySupply> ToiletrySupplies { get; set; }
        public virtual ICollection<ExtraToiletrySupply> ExtraToiletrySupplies { get; set; }

        //periodical
        public virtual ICollection<Periodical.Periodical> Periodicals { get; set; }
        public virtual ICollection<ExtraPeriodical> ExtraPeriodicals { get; set; }

        [Required]
        public bool IsSameAddress { get; set; }

        public decimal? PricePa { get; set; }

        public decimal? ReturnPw { get; set; }

        public string FileName { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? LastUpdateDate { get; set; }

        [Required]
        public CostType CostType { get; set; }

        [Required]
        public CostStatus Status { get; set; }

        //costing
        public int? RegCleanWeeks { get; set; }
        public int? AdminCleanWeeks { get; set; }

        public decimal? AdminSubcontractorCostPw { get; set; }
        public decimal? RegSubcontractorCostPw { get; set; }


        public decimal? AdminPeriodicalCostPw { get; set; }
        public decimal? RegPeriodicalCostPw { get; set; }

        public decimal? AdminSupplyCostPw { get; set; }
        public decimal? RegSupplyCostPw { get; set; }

        public decimal? AdminLabourCostPw { get; set; }
        public decimal? RegLabourCostPw { get; set; }


        //Equipment total price should always multiply 52 weeks
        public decimal? RegEquipmentCostPw { get; set; }
        
        public decimal? RegLabourMiscCostPw { get; set; }
        public decimal? SubTotal { get; set; }
        public decimal? Total { get; set; }
        public decimal? Return { get; set; }
        public decimal? PriceForClient { get; set; }
        public decimal? Liability { get; set; }
        public decimal? Margin { get; set; }

        //Basic Information
        public bool? IsAdminClean { get; set; }

        [ForeignKey("PublicLiability")]
        public int? PublicLiabilityId { get; set; }
        public PublicLiability PublicLiability { get; set; }
        //Source of Quotation
        [ForeignKey("QuoteSource")]
        public int? QuoteSourceId { get; set; }
        public QuoteSource QuoteSource { get; set; }
        public string Spec { get; set; }
        [ForeignKey("IndustryType")]
        public int? IndustryTypeId { get; set; }
        public CleaningSpec IndustryType { get; set; }

        public bool? ColorCode { get; set; }
        public bool? CleanOnHoliday { get; set; }
        public int? NoOfHoliday { get; set; }

        [ForeignKey("StandardRegion")]
        public int? StandardRegionId { get; set; }
        public StandardRegion StandardRegion { get; set; }

        public bool? CheckedWithClient { get; set; }
        public bool? IsPriceChanged { get; set; }
        public bool? DiscussWithClient { get; set; }
        public string WorkDoneWithin { get; set; }
        public string AreaDescription { get; set; }
        public string WorkDescription { get; set; }
        public int DaysCleanPerWeek { get; set; }
        public DayOfClean DayOfClean { get; set; }
        public bool? Fortnightly { get; set; }
        public bool? Monthly { get; set; }
    }
}