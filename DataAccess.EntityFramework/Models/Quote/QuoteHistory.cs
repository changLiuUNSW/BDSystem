using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.EntityFramework.Models.Quote
{
    [Table("QuoteHistory", Schema = "Quote")]
    public class QuoteHistory
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Description { get; set; }
        public DateTime Time { get; set; }
        public string User { get; set; }


        [ForeignKey("FromStatus")]
        public int FromStatusId { get; set; }

        [ForeignKey("ToStatus")]
        public int ToStatusId { get; set; }

        public virtual QuoteStatus FromStatus { get; set; }

        public virtual QuoteStatus ToStatus { get; set; }

        [ForeignKey("Quote")]
        public int QuoteId { get; set; }

        public virtual Quote Quote { get; set; }
    }
}