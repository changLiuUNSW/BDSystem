using System.ComponentModel.DataAnnotations.Schema;
namespace DataAccess.EntityFramework.Models.Quote.Cost
{
    [ComplexType]
    public class DayOfClean
    {
        [Column("Monday")]
        public bool? Monday { get; set; }

        [Column("Tuesday")]
        public bool? Tuesday { get; set; }

        [Column("Wednesday")]
        public bool? Wednesday { get; set; }

        [Column("Thursday")]
        public bool? Thursday { get; set; }

        [Column("Friday")]
        public bool? Friday { get; set; }

        [Column("Saturday")]
        public bool? Saturday { get; set; }

        [Column("Sunday")]
        public bool? Sunday { get; set; }
    }
}