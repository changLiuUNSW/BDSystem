using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Permissions;

namespace DataAccess.EntityFramework.Models.Quote.Cost.Labour
{
    [Table("OnCostRate", Schema = "Quote")]
    public class OnCostRate
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string State { get; set; }
        public decimal HolidayPay { get; set; }
        public decimal SickPay { get; set; }
        public decimal WorkerCompensation { get; set; }
        public decimal Superannuation { get; set; }
        public decimal PayrollTax { get; set; }
        public decimal LongService { get; set; }
        public decimal IncomeProtection { get; set; }
        public decimal Materials { get; set; }
    }
}
