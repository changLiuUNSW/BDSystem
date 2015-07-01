using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.EntityFramework.Models.BD.Contact
{
    [Table("BusinessType", Schema = "BD")]
    public class BusinessType
    {
        [Key]
        public int Id { get; set; }
        public string Type { get; set; }

        //excessive lazy loading in lookup table
        /*public virtual ICollection<Contact> Contacts { get; set; }
        public virtual ICollection<Lead.Lead> Leads { get; set; }
        public virtual ICollection<Quote.Quote> Quotes { get; set; }*/
    }
}
