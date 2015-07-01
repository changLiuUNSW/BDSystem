using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.EntityFramework.Models.Quote.Cost
{
    [Table("Margin", Schema = "Quote")]
    public class Margin
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public decimal Cost { get; set; }
        public decimal MarginInDollar { get; set; }
        public double MarginInPercent { get; set; }
    }
}
