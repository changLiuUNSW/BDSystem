using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.EntityFramework.Models.Quote.Cost.Supply
{
    [Table("ToiletrySupply", Schema = "Quote")]
    public class ToiletrySupply
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("Cost")]
        public int CostId { get; set; }
        public virtual Cost Cost { get; set; }

        public int? UnitsPw { get; set; }

        [ForeignKey("ToiletRequisite")]
        public string ToiletryCode { get; set; }
        public virtual ToiletRequisite ToiletRequisite { get; set; }

        public decimal? Total { get; set; }
    }
}
