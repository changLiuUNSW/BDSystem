using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.EntityFramework.Models.BD.Site
{
    [Table("CleaningContract", Schema = "BD")]
    public class CleaningContract
    {
        [Key, ForeignKey("Site")]
        public int SiteId { get; set; }

        [Required]
        public virtual Site Site { get; set; }

        [MaxLength(40)]
        public string Contractor { get; set; }
        public string ContactDuringQuote { get; set; }

        public decimal PricePa { get; set; }
        public string UnsuccessReason { get; set; }

        [Column(TypeName="Date")]
        public DateTime? ReviewDate { get; set; }
        [Column(TypeName = "Date")]
        public DateTime? DateQuoted { get; set; }
        public double? CleaningFreq { get; set; }
        public double? QualifyingQuantity { get; set; }
    }
}

