using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.EntityFramework.Models.Quote.Cost.Labour
{
    public enum LabourCostOptions
    {
        Weekdays,
        Saturday,
        Sunday,
        Holiday
    }

    [Table("LabourEstimation", Schema = "Quote")]
    public class LabourEstimation
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("Labour")]
        public int LabourId { get; set; }
        public virtual Labour Labour { get; set; }

        [ForeignKey("LabourRate")]
        public int LabourRateId { get; set; }
        public virtual LabourRate LabourRate { get; set; }

        public int? DaysPerWeek { get; set; }
        public int? MinsOnWeekdays { get; set; }
        public int? MinsOnSat { get; set; }
        public int? MinsOnSun { get; set; }
        public int? MinsOnHoliday { get; set; }

        public decimal? WeekdayTotal { get; set; }
        public decimal? SaturdayTotal { get; set; }
        public decimal? SundayTotal { get; set; }
        public decimal? HolidayTotal { get; set; }

        public decimal? Total { get; set; }

        public decimal HolidayFactor { get; set; }
    }
}
