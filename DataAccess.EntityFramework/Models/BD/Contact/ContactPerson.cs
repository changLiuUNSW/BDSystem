using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.EntityFramework.Models.BD.Contact
{
    [Table("ContactPerson", Schema = "BD")]
    public class ContactPerson
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("Site")]
        public int SiteId { get; set; }
        public virtual Site.Site Site { get; set; }

        public virtual ICollection<Contact> Contacts { get; set; }
        public virtual ICollection<ContactPersonHistory> Histories { get; set; }

        public string Title { get; set; }

        public string Firstname { get; set; }
        public string Lastname { get; set; }
         
        public string Email { get; set; }
        public string Position { get; set; }
        public string Mobile { get; set; }
        public string Fax { get; set; }
        public string DirectLine { get; set; }
        public string PoNumber { get; set; }
        public string PoUnit { get; set; }
        public string PoStreet { get; set; }
        public string PoSuburb { get; set; }
        public string PoState { get; set; }
        public string PoPostcode { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
