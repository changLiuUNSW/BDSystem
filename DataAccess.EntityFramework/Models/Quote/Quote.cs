using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using DataAccess.EntityFramework.Models.BD;
using DataAccess.EntityFramework.Models.BD.Allocation;
using DataAccess.EntityFramework.Models.BD.Contact;
using DataAccess.EntityFramework.Models.BD.Lead;
using DataAccess.EntityFramework.TypeLibrary;

namespace DataAccess.EntityFramework.Models.Quote
{
    [Table("Quote", Schema = "Quote")]
    public class Quote
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("Lead")]
        public int? LeadId { get; set; }
        public virtual Lead Lead { get; set; }
        [ForeignKey("LeadPersonal")]
        public int LeadPersonalId { get; set; }
        public virtual LeadPersonal LeadPersonal { get; set; }

        public virtual ICollection<QuoteHistory> Histories { get; set; }
        public virtual ICollection<QuoteIssue> QuoteIssues { get; set; }
        public virtual ICollection<Cost.Cost> QuoteCost { get; set; }
        public virtual ICollection<QuoteQuestionResult> QuestionResults { get; set; }

        [ForeignKey("Status")]
        public int StatusId { get; set; }
        public virtual QuoteStatus Status { get; set; }


        [ForeignKey("BusinessType")]
        public int BusinessTypeId { get; set; }
        public virtual BusinessType BusinessType { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public DateTime? PrintDate { get; set; }
        public DateTime? DueDate { get; set; }

        public string Company { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Title { get; set; }
        public string Position { get; set; }
        public string Phone { get; set; }
        public Address Address { get; set; }

        public DateTime? LastUploadDate { get; set; }
        public string FileName { get; set; }
        public string PricePageName { get; set; }

        public int? SuccessRate { get; set; }

        public bool BDReview { get; set; }
        public bool BDGMReview { get; set; }

        public decimal? LastestPA { get; set; }

        [MaxLength(25)]
        [ForeignKey("SalesBox")]
        [Column(Order = 1)]
        public string State { get; set; }

        [MaxLength(9)]
        [ForeignKey("SalesBox")]
        [Column(Order = 0)]
        public string Postcode { get; set; }
        public virtual SalesBox SalesBox { get; set; } 
        public virtual WpRequiredInfo WpRequiredInfo { get; set; }
        public DateTime? LastContactDate { get; set; }

        public bool ContactCheckOverDue { get; set; }
        public bool DeadCheckOverDue { get; set; }
        public bool AjustCheckOverDue { get; set; }

        public bool WasCurrent { get; set; }

        public DateTime? LastContactCheckDate { get; set; }
        public DateTime? LastDeadCheckDate { get; set; }
        public DateTime? LastAdjustCheckDate { get; set; }

        [NotMapped]
        public decimal? TotalPA
        {
            get
            {
                if (QuoteCost != null)
                {
                    return LastestPA ?? QuoteCost.Sum(l => l.PricePa);
                }
                return LastestPA;
            }
        }

        [NotMapped]
        public decimal? TotalPW
        {
            get
            {
                if (QuoteCost != null)
                {
                    return QuoteCost.Sum(l => l.ReturnPw);
                }
                return null;
            }
        }
    }
}