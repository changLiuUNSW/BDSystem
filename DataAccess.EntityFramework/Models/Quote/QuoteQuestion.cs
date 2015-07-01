using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DataAccess.EntityFramework.TypeLibrary;

namespace DataAccess.EntityFramework.Models.Quote
{
    [Table("Question", Schema = "Quote")]
    public class QuoteQuestion
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public QuoteQuestionType Type { get; set; }

        public bool TextOnly { get; set; }

        public virtual ICollection<QuoteAnswer> QuoteAnswers { get; set; }
    }
}