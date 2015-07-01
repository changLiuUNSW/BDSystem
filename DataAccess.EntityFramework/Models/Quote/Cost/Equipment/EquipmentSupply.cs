using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.EntityFramework.Models.Quote.Cost.Equipment
{
    [Table("EquipmentSupply", Schema = "Quote")]
    public class EquipmentSupply
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("Cost")]
        public int CostId { get; set; }
        public virtual Cost Cost { get; set; }

        [ForeignKey("Equipment")]
        public string EquipmentCode { get; set; }
        public virtual Equipment Equipment { get; set; }

        public int? UnitsFromSubby { get; set; }
        public int? UnitsFromQuad { get; set; }

        public decimal? Total { get; set; }
    }
}
