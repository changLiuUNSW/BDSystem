using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.EntityFramework.Models.BD.Lead
{
    [Table("LeadGroup", Schema = "BD")]
    public class LeadGroup
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required, MaxLength(3)]
        public string Group { get; set; }
        public int LeadTarget { get; set; }
        public int LeadStop { get; set; }
    }
}
