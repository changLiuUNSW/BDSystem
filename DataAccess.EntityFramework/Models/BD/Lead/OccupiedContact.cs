using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.EntityFramework.Models.BD.Lead
{
    [Table("OccupiedContact", Schema = "BD")]
    public class OccupiedContact
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("Contact")]
        public int ContactId { get; set; }
        public virtual Contact.Contact Contact { get; set; }

        [ForeignKey("Telesale")]
        public int TelesaleId { get; set; }
        public virtual Telesale.Telesale Telesale { get; set; }

        [ForeignKey("LeadPersonal")]
        public int? LeadPersonalId { get; set; }
        public virtual LeadPersonal LeadPersonal { get; set; }

        public DateTime TimeStarted { get; set; }
    }
}
