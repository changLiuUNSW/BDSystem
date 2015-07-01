using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.EntityFramework.Models.BD
{
    [ComplexType]
    public class Address
    {
        [MaxLength(20), Column("Number")]
        public string Number { get; set; }

        [MaxLength(20), Column("Unit")]
        public string Unit { get; set; }

        [MaxLength(50), Column("Street")]
        public string Street { get; set; }

        [MaxLength(25), Column("Suburb")]
        public string Suburb { get; set; }


    }
}
