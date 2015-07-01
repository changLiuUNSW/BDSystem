using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.EntityFramework.Models.Quote.Cost.Equipment
{
    [Table("Machine", Schema = "Quote")]
    public class Machine
    {
        [Key]
        public string Type { get; set; }

        public int YearsAllocated { get; set; }
        public double RepairFactor {get; set; }
        public int Fuels { get; set; }
    }
}
