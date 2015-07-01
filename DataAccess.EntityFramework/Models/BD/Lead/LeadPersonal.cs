using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using DataAccess.EntityFramework.Extensions.Utilities;
using Newtonsoft.Json;

namespace DataAccess.EntityFramework.Models.BD.Lead
{

    [Table("LeadPersonal", Schema = "BD")]
    public class LeadPersonal
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Initial { get; set; }

        [Required]
        public string Name { get; set; }

        [Required, ForeignKey("LeadGroup")]
        public int GroupId { get; set; }

        public virtual LeadGroup LeadGroup { get; set; }
        [JsonIgnore]
        public virtual ICollection<Allocation.Allocation> Allocations { get; set; }
        public virtual ICollection<Quote.Quote> Quotes { get; set; }
        public virtual ICollection<Lead> Leads { get; set; }
        public int? LeadStop { get; set; }
        public int? LeadTarget { get; set; }
        public int? TempLeadStop { get; set; }
        public int? TempLeadTarget { get; set; }

        [DefaultValue(false)]
        public bool PriorityToCall { get; set; }

        [Column(TypeName="Date")]
        public DateTime? LeadsOnHoldDate { get; set; }
        public string LeadsOnHoldReason { get; set; }

        [JsonIgnore]
        public ICollection<OccupiedContact> OccupiedContacts { get; set; } 

        /// <summary>
        /// return whether the qp has lead number over its lead stop limit
        /// </summary>
        /// <returns></returns>
        public bool IsOverLeadStop()
        {
            var leadStop = GetLeadStop() - Leads.Count;

            if (leadStop > 0)
                return false;

            return true;
        }

        /// <summary>
        /// return whether the qp has been put on hold for not getting any lead
        /// </summary>
        /// <returns></returns>
        public bool IsOnHold()
        {
            if (LeadsOnHoldDate == null)
                return false;

            if (LeadsOnHoldDate < DateTime.Today.Date)
                return false;

            return true;
        }

        /// <summary>
        /// the priority of leadStop is TempLeadStop > individual leadStop > GMGroup leadStop
        /// </summary>
        /// <returns></returns>
        public int GetLeadStop()
        {
            if (TempLeadStop != null)
                return (int) TempLeadStop;

            if (LeadStop != null)
                return (int) LeadStop;

            return LeadGroup.LeadStop;
        }

        /// <summary>
        /// priority of lead target is temp > individual lead target > gm group lead target
        /// </summary>
        /// <returns></returns>
        public int GetLeadTarget()
        {
            if (TempLeadTarget != null)
                return (int) TempLeadTarget;

            if (LeadTarget != null)
                return (int) LeadTarget;

            return LeadGroup.LeadTarget;
        }

        /// <summary>
        /// return number of leads left to get for this week
        /// </summary>
        /// <returns></returns>
        public int GetLeadsLeftToGet()
        {
            var monday = DateExtension.Monday();
            var sunday = DateExtension.Sunday();

            var num = GetLeadTarget() - Leads.Count(x=>x.CreatedDate >= monday && x.CreatedDate <= sunday);

            return num < 0 ? 0 : num;
        }

        /// <summary>
        /// return number of contacts needed per year base on current lead shift information and lead target
        /// </summary>
        /// <param name="shiftInfo"></param>
        /// <returns></returns>
        public int GetYearlyContactNeeds(LeadShiftInfo shiftInfo)
        {
            if (shiftInfo == null)
                throw new ArgumentNullException("shiftInfo");

            return shiftInfo.ContactRatePer3HrShift * shiftInfo.CallCycleWeeks * GetLeadTarget();
        }

        public int GetWeeklyContactNeeds(LeadShiftInfo shiftInfo)
        {
            if (shiftInfo == null)
                throw new ArgumentNullException("shiftInfo");

            return shiftInfo.RecordPerShift * GetLeadsLeftToGet();
        }
    }
}
