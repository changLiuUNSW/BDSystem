using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.EntityFramework.Models.Quote.Cost.Vacation
{
    [Table("VacationCleaning", Schema = "Quote")]
    public class VacationCleaning
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("Cost")]
        public int CostId { get; set; }
        public virtual Cost Cost { get; set; }

        public string Description { get; set; }
        public int? WeeksToClean { get; set; }
        public decimal? SubbyCost { get; set; }

        /// <summary>
        /// return weekly Cost on current vacation cleaning detail
        /// </summary>
        /// <returns></returns>
        public decimal TotalCost()
        {
            if (WeeksToClean.GetValueOrDefault() <= 0 || SubbyCost.GetValueOrDefault() <= 0)
                return 0;

            return WeeksToClean.GetValueOrDefault()*SubbyCost.GetValueOrDefault();
        }
    }
}
