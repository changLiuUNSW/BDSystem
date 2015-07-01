using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace DataAccess.EntityFramework.Models.Quote
{
    [Table("Status", Schema = "Quote")]
    public class QuoteStatus
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
        public bool Hidden { get; set; }

        [NotMapped]
        public int Count
        {
            get {
                return Quotes == null ? 0 : Quotes.Count;
            }
        }

        [JsonIgnore]
        public virtual ICollection<Quote> Quotes { get; set; }
    }
}
