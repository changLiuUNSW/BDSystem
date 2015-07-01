using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.EntityFramework.Models.Quote.Cost.Labour
{
    [Table("LabourRate", Schema = "Quote")]
    public class LabourRate
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public decimal Weekdays { get; set; }
        public decimal Saturday { get; set; }
        public decimal Sunday { get; set; }
        public decimal Holiday { get; set; }
    }

}
