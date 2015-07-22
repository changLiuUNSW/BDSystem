using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace DataAccess.EntityFramework.Models.Quote
{
    [Table("QuestionResult", Schema = "Quote")]
    public class QuoteQuestionResult
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("Quote")]
        public int QuoteId { get; set; }
        public virtual Quote Quote { get; set; }

        [ForeignKey("Question")]
        public int QuestionId { get; set; }
        public virtual QuoteQuestion Question { get; set; }

        [ForeignKey("Answer")]
        public int? AnswerId { get; set; }
        public virtual QuoteAnswer Answer { get; set; }

        public DateTime Time { get; set; }

        public string User { get; set; }

        public string Additional { get; set; }
    }
}
