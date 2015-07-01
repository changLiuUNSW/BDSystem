using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DataAccess.EntityFramework.Models.BD.Site;

namespace DataAccess.EntityFramework.Models.BD.Contact
{
    /// <summary>
    /// contact table
    /// </summary>
    [Table("Contact", Schema = "BD")]
    public class Contact
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public virtual ICollection<Lead.Lead> Leads { get; set; }
        public virtual ICollection<CallLine> CallLines { get; set; }

        [ForeignKey("Site")]
        public int SiteId { get; set; }
        public virtual Site.Site Site { get; set; }
      
        [ForeignKey("BusinessType")]
        public int BusinessTypeId { get; set; }
        public virtual BusinessType BusinessType { get; set; }

        [ForeignKey("ContactPerson")]
        public int? ContactPersonId { get; set; }
        public virtual ContactPerson ContactPerson { get; set; }

        [Column(TypeName="Date")]
        public DateTime? NextCall { get; set; }

        [Column(TypeName="Date")]
        public DateTime? LastCall { get; set; }

        [Column(TypeName="Date")]
        public DateTime? NewManagerDate { get; set; }
        
        public bool DaToCheck { get; set; }
        public string DaToCheckInfo { get; set; }

        public bool ExtManagement { get; set; }
        public bool ReceptionName { get; set; }

        [MaxLength(3)]
        public string Code { get; set; }
        public string Note { get; set; }

        public short? CallFrequency { get; set; }

        public DateTime? CallBackDate { get; set; }
    }
}
