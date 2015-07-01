using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.EntityFramework.Models.Quote.Cost.Periodical
{
    [Table("ExtraPeriodical", Schema = "Quote")]
    public class ExtraPeriodical
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("Cost")]
        public int CostId { get; set; }
        public virtual Cost Cost { get; set; }

        public string Description { get; set; }
        public decimal? CostPerTime { get; set; }
        public decimal? MarginPerTime { get; set; }
    }
}
