using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.EntityFramework.Models.BD.Contact
{
    [Table("ContactPersonHistory", Schema = "BD")]
    public class ContactPersonHistory
    {
        //history table use the key from its original table
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("ContactPerson")]
        public int OriginalContactPersonId { get; set; }
        public virtual ContactPerson ContactPerson { get; set; }

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

        public DateTime Time { get; set; }
        public string EditName { get; set; }
        public string ReasonForChange { get; set; }
    }
}
