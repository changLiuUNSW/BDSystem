using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.EntityFramework.Models.BD.Lead
{
     [Table("LeadHistory", Schema = "BD")]
    public class LeadHistory
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

         public virtual LeadStatus FromStatus { get; set; }

         public virtual LeadStatus ToStatus { get; set; }

         [ForeignKey("Lead")]
         public int LeadId { get; set; }
         public virtual Lead Lead { get; set; }
    }
}