using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DataAccess.EntityFramework.Models.BD.Allocation;
using DataAccess.EntityFramework.Models.BD.Contact;

namespace DataAccess.EntityFramework.Models.BD.Lead
{
    [Table("Lead", Schema = "BD")]
    public  class Lead
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("Telesale")]
        public int TelesaleId { get; set; }
        public virtual Telesale.Telesale Telesale { get; set; }

        public virtual ICollection<Quote.Quote> Quotes { get; set; }

        [ForeignKey("LeadPersonal")]
        public int LeadPersonalId { get; set; }
        public virtual LeadPersonal LeadPersonal { get; set; }

//        public string LeadType { get; set; }
        [ForeignKey("BusinessType")]
        public int BusinessTypeId { get; set; }
        public virtual BusinessType BusinessType { get; set; }

        public Address Address { get; set; }

        [MaxLength(25)]
        [ForeignKey("SalesBox")]
        [Column(Order = 1)]
        public string State { get; set; }

        [MaxLength(9)]
        [ForeignKey("SalesBox")]
        [Column(Order = 0)]
        public string Postcode { get; set; }

        public virtual SalesBox SalesBox { get; set; } 

        [MaxLength(15)]
        [Required]
        public string Phone { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
        [ForeignKey("LeadStatus")]
        public int LeadStatusId { get; set; }
        public virtual LeadStatus LeadStatus { get; set; }

        [ForeignKey("Contact")]
        public int ContactId { get; set; }
        public virtual Contact.Contact Contact { get; set; }
        public virtual ICollection<LeadHistory> Histories { get; set; }

        public DateTime? AppointmentDate { get; set; }
        public DateTime? CallBackDate { get; set; }
    }
}
