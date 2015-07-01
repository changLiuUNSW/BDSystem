using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DataAccess.Common.Contact;

namespace DataAccess.EntityFramework.Models.BD.Contact
{
    [Table("FullReport", Schema = "BD")]
    public class FullReport
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public virtual ICollection<FullReportDetail> Details { get; set; }  

    }

    [Table("FullReportDetail", Schema = "BD")]
    public class FullReportDetail : ContactSummary
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int ReportId { get; set; }

        public int? CallFreq { get; set; }
        public int DaToCheck { get; set; }
        public int NoName { get; set; }
        public int ReCall { get; set; }
        public int Inhouse { get; set; }
        public int NonInhouse { get; set; }
        public int Total { get; set; }
        public string Size { get; set; }
    }
}
