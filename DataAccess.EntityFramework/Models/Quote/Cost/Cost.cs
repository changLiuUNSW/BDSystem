using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DataAccess.EntityFramework.Models.BD;
using DataAccess.EntityFramework.Models.BD.Allocation;
using DataAccess.EntityFramework.Models.Quote.Cost.Equipment;
using DataAccess.EntityFramework.Models.Quote.Cost.Periodical;
using DataAccess.EntityFramework.Models.Quote.Cost.Supply;
using DataAccess.EntityFramework.TypeLibrary;

namespace DataAccess.EntityFramework.Models.Quote.Cost
{
    [Table("Cost", Schema = "Quote")]
    public class Cost
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("Quote")]
        public int QuoteId { get; set; }

        public virtual Quote Quote { get; set; }

        [ForeignKey("Labour")]
        public int LabourId { get; set; }
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

        [MaxLength(25)]
        [ForeignKey("SalesBox")]
        [Column(Order = 1)]
        public string State { get; set; }

        [MaxLength(9)]
        [ForeignKey("SalesBox")]
        [Column(Order = 0)]
        public string Postcode { get; set; }

        public string Company { get; set; }

        public Address Address { get; set; }

        public virtual SalesBox SalesBox { get; set; }

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
        public decimal? RegSubcontractorCostPw { get; set; }
        public decimal? RegPeriodicalCostPw { get; set; }
        public decimal? RegSupplyCostPw { get; set; }
        public decimal? RegEquipmentCostPw { get; set; }
        public decimal? RegLabourCostPw { get; set; }
        public decimal? RegLabourMiscCostPw { get; set; }
        public decimal? SubTotal { get; set; }
        public decimal? Total { get; set; }
        public decimal? Return { get; set; }
        public decimal? PriceForClient { get; set; }
        public decimal? Liability { get; set; }
        public decimal? Margin { get; set; }

    }
}