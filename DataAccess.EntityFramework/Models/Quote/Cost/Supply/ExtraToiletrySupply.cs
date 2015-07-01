using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.EntityFramework.Models.Quote.Cost.Supply
{
    [Table("ExtraToiletrySupply", Schema = "Quote")]
    public class ExtraToiletrySupply
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("Cost")]
        public int CostId { get; set; }
        public virtual Cost Cost { get; set; }

        [ForeignKey("ToiletRequisite")]
        public string Code { get; set; }
        public virtual ToiletRequisite ToiletRequisite { get; set; }

        public decimal? CostPerUnit { get; set; }
        public int? UnitPerCarton { get; set; }
    }
}
