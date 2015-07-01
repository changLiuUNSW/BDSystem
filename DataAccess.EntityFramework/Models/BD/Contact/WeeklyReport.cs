using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DataAccess.Common.Contact;

namespace DataAccess.EntityFramework.Models.BD.Contact
{
    [Table("WeeklyReport", Schema = "BD")]
    public class WeeklyReport
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public virtual ICollection<WeeklyReportDetail> Details { get; set; }
    }

    [Table("WeeklyReportDetail", Schema = "BD")]
    public class WeeklyReportDetail : ContactSummary
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        public int ReportId { get; set; }

        public int Overdue { get; set; }
        public int OverdueAndReady { get; set; }
        public int Total { get; set; }
    }
}
