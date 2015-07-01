using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DataAccess.EntityFramework.Models.BD.Allocation;
using DataAccess.EntityFramework.Models.BD.Contact;

namespace DataAccess.EntityFramework.Models.BD.Site
{
    /// <summary>
    /// SITE table
    /// </summary>
    [Table("Site", Schema = "BD")]
    public class Site
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Key { get; set; }

        public virtual ICollection<Contact.Contact> Contacts { get; set; }
        public virtual ICollection<ContactPerson> ContactPersons { get; set; }
        public virtual ICollection<SiteGroup> Groups { get; set; }

        public virtual CleaningContract CleaningContract { get; set; }
        public virtual SecurityContract SecurityContract { get; set; }
        public virtual SalesBox SalesBox { get; set; } 

        [MaxLength(20)]
        public string Number { get; set; }

        [MaxLength(20)]
        public string Unit { get; set; }

        [MaxLength(50)]
        public string Street { get; set; }

        [MaxLength(25)]
        public string Suburb { get; set; }

        [MaxLength(25)]
        [ForeignKey("SalesBox")]
        [Column(Order=1)]
        public string State { get; set; }

        [MaxLength(9)]
        [ForeignKey("SalesBox")]
        [Column(Order=0)]
        public string Postcode { get; set; }

        public string Name { get; set; }

        [MaxLength(50)]
        public string BuildingName { get; set; }
        public bool PropertyManaged { get; set; }
        
        //[MaxLength(50)]
        //public string PropertyManagerName { get; set; }
        public bool PMSite { get; set; }

        [ForeignKey("BuildingType")]
        public int? BuildTypeId { get; set; }
        public virtual BuildingType BuildingType { get; set; }

        [MaxLength(15)]
        public string Phone { get; set; }

        public double? Qualification { get; set; }

        public string Size { get; set; }

        public bool InHouse { get; set; }
        public bool TsToCall { get; set; }
    }
}