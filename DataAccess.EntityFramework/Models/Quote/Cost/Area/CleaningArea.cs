using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.EntityFramework.Models.Quote.Cost.Area
{
    [Table("CleaningArea", Schema = "Quote")]
    public class CleaningArea
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /*[ForeignKey("Cost")]
        public int CostId { get; set; }
        public virtual Cost.Cost Cost { get; set; }*/

        public string Name { get; set; }
        public bool? Include { get; set; }
        public bool? Exclude { get; set; }
    }
}
