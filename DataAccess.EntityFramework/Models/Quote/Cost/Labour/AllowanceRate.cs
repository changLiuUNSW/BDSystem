using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.EntityFramework.Models.Quote.Cost.Labour
{
    [Table("AllowanceRate", Schema = "Quote")]
    public class AllowanceRate
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string State { get; set; }
        public decimal ToiletAllowPerShift { get; set; }
        public decimal LeadingHandSmallGroup { get; set; }
        public decimal LeadingHandLargeGroup { get; set; }
        public decimal NumberOfHolidays { get; set; }
    }
}
