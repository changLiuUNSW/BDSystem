using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.EntityFramework.Models.Quad
{
    [Table("PhoneBook", Schema = "Quad")]
    public class QuadPhoneBook
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string LoginName { get; set; }
        public string Intial { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Position { get; set; }
        public string ManagerInitial { get; set; }
        public string DirectNumber { get; set; }
        public string Ext { get; set; }
        public string SpeedDail { get; set; }
        public string Mobile { get; set; }
        public string Fax { get; set; }
        public string State { get; set; }
        public string Group { get; set; }
        public string QuadArea { get; set; }
        public string LeadArea { get; set; }
        public string Email { get; set; }
    }
}
