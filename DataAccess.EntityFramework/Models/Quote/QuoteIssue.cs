using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.EntityFramework.Models.Quote
{
    [Table("Issue", Schema = "Quote")]
    public class QuoteIssue
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
    
        [ForeignKey("Quote")]
        public int QuoteId { get; set; }
        public virtual Quote Quote { get; set; }
        public bool Resolved { get; set; }

        [ForeignKey("NextStatus")]
        public int NextStatusId { get; set; }
        public virtual QuoteStatus NextStatus { get; set; }


        [Required]
        public string CreatedBy { get; set; }

        [Required]
        public string IssueDetail { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool UploadRequired { get; set; }

        public bool UploadPricePageOnly { get; set; }

        public bool IsEmailOnly { get; set; }


        public string SolvedBy { get; set; }

        public DateTime? SolvedDate { get; set; }

        public string SolverComments { get; set; }
    }
}