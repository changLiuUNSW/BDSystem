using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.EntityFramework.Models.BD.Site
{
    [Table("SecurityContract", Schema = "BD")]
    public class SecurityContract
    {
        [Key, ForeignKey("Site")]
        public int SiteId { get; set; }

        [Required]
        public virtual Site Site { get; set; }

        [MaxLength(40)]
        public string Contarctor { get; set; }

        [Column(TypeName = "Date")]
        public DateTime? ReviewDate { get; set; }

        public bool GuardingPersonnel { get; set; }
        public bool MobilePatrol { get; set; }
        public bool Conceirge { get; set; }
        public bool ElectronicInstallation { get; set; }
        public bool BackToBaseMonitoring { get; set; }
        public bool SecurityMaintenance { get; set; }

        /*public decimal PricePa { get; set; }
        public string UnsuccessReason { get; set; }*/
    }
}
