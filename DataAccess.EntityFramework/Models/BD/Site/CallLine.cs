using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DataAccess.EntityFramework.TypeLibrary;

namespace DataAccess.EntityFramework.Models.BD.Site
{

    [Table("CallLine", Schema = "BD")]
    public class CallLine
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Initial { get; set; }

        [ForeignKey("Contact")]
        public int ContactId { get; set; }
        public virtual Contact.Contact Contact { get; set; }
        public bool Visit { get; set; }
        public bool Call { get; set; }
        public bool Lunch { get; set; }
        public bool Entertainment { get; set; }
        public string EntertainmentEvent { get; set; }
        public bool Profile { get; set; }
        public bool DirectMail { get; set; }
        public bool Lead { get; set; }
        public bool EmailInfo { get; set; }
        public CallLineStatus Status { get; set; }
        public DateTime LastContact { get; set; }
        public DateTime TimeCreated { get; set; }
    }
}
