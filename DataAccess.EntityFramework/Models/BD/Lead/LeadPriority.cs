using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.EntityFramework.Models.BD.Lead
{
    [Table("LeadPriority", Schema = "BD")]
    public class LeadPriority
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Role { get; set; }
        public string Priority { get; set; }
        public int? Distance { get; set; }
    }
}
