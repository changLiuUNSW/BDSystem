using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DataAccess.EntityFramework.Models.BD.Lead;

namespace DataAccess.EntityFramework.Models.BD.Allocation
{
    [Table("Allocation", Schema = "BD")]
    public class Allocation
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("LeadPersonal")]
        public int LeadPersonalId { get; set; }

        public virtual LeadPersonal LeadPersonal { get; set; }
        [Required]
        public string Initial { get; set; }
        [Required]
        public string Size { get; set; }
        [Required]
        public string Zone { get; set; }
    }
}
