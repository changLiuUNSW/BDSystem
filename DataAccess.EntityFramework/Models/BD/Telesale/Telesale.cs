using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.EntityFramework.Models.BD.Telesale
{
    [Table("Telesale", Schema = "BD")]
    public class Telesale
    {
        [Key]
        public int Id { get; set; }
        public string Initial { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Assignment> Assignments { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
