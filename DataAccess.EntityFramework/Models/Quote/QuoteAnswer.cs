using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DataAccess.EntityFramework.TypeLibrary;

namespace DataAccess.EntityFramework.Models.Quote
{
    [Table("Answer", Schema = "Quote")]
    public class QuoteAnswer
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public QuoteAnswerType Type { get; set; }

        [ForeignKey("Question")]
        public int QuestionId { get; set; }
        public virtual QuoteQuestion Question { get; set; }

        
    }
}