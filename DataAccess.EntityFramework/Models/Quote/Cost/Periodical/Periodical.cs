using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.EntityFramework.Models.Quote.Cost.Periodical
{
    [Table("Periodical", Schema = "Quote")]
    public class Periodical
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("Cost")]
        public int CostId { get; set; }
        public virtual Cost Cost { get; set; }

        public string Description { get; set; }

        public decimal? CostPerTime { get; set; }
        public int? FreqPa { get; set; }
        public bool? Itemise { get; set; }
    }
}
