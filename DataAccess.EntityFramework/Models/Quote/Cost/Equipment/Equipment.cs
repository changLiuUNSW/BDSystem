using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.EntityFramework.Models.Quote.Cost.Equipment
{
    [Table("Equipment", Schema = "Quote")]
    public class Equipment
    {
        [Key]
        public string EquipmentCode { get; set; }
        public string Description { get; set; }

        [ForeignKey("Machine")]
        public string MachineType { get; set; }
        public virtual Machine Machine { get; set; }
        
        public decimal Cost { get; set; }
        public string UserGuide { get; set; }
        public string Comment { get; set; }
        public int ProductionRatePerHour { get; set; }
        public DateTime UpdateDate { get; set; }
        public string Issues { get; set; }
        public bool IsLargeEquipment { get; set; }
    }
}
