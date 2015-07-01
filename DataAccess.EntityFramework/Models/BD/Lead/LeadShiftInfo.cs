using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.EntityFramework.Models.BD.Lead
{
    [Table("LeadShiftInfo", Schema = "BD")]
    public class LeadShiftInfo
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int RecordPerShift { get; set; }
        public double LeadPerShift { get; set; }
        public int ContactRatePer3HrShift { get; set; }
        public int CallCycleWeeks { get; set; }
        public int PaCalledWks { get; set; }
    }
}
