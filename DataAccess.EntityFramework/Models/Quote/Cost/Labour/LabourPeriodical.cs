using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.EntityFramework.Models.Quote.Cost.Labour
{
    [Table("LabourPeriodicals", Schema = "Quote")]
    public class LabourPeriodical
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("Labour")]
        public int LabourId { get; set; }
        public virtual Labour Labour { get; set; }

        [ForeignKey("LabourRate")]
        public int LabourRateId { get; set; }
        public virtual LabourRate LabourRate { get; set; }

        [DefaultValue(true)]
        public bool AddToPricePage { get; set; }

        public int? Hours { get; set; }
        public int? Frequency { get; set; }

        //Cost
        public decimal? Material { get; set; }
        public decimal? Wage { get; set; }


        public int? WeeksToInvoice { get; set; }
    }
}
