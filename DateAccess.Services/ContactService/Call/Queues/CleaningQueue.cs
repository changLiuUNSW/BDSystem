using System.Collections.Generic;
using System.Linq;
using DataAccess.EntityFramework.Extensions.Utilities;
using DataAccess.EntityFramework.Models.BD.Lead;
using DataAccess.EntityFramework.TypeLibrary;
using DateAccess.Services.ContactService.Call.Models;

namespace DateAccess.Services.ContactService.Call.Queues
{

    /// <summary>
    /// this is the cleaning queue used for OPR cleaning call
    /// </summary>
    public class CleaningQueue : IQueue<Dictionary<LeadPersonal, string>>
    {
        struct Limit
        {
            public int Role { get; set; }
            public int? Max { get; set; }
            public int? Min { get; set; }
        }

        private IList<Limit> Limits { get; set;} 
        public IList<LeadPersonal> Persons { get; set; }
        public IList<LeadPriority> Priorities { get; set; }
        public PriorityConfig Config { get; set; } 

        public CleaningQueue(IList<LeadPersonal> persons, IList<LeadPriority> priorities, PriorityConfig config)
        {

            Config = config;
            Persons = persons;
            Priorities = priorities;

            Limits = Priorities
                .Where(x => x.Distance != null)
                .GroupBy(x => x.Role)
                .Select(x => new Limit
            {
                Role = x.Key,
                Min = x.Min(y => y.Distance),
                Max = x.Max(y => y.Distance)
            }).ToList();
        }

        /// <summary>
        /// return priority value for all avaiable lead personal
        /// </summary>
        /// <returns></returns>
        public Dictionary<LeadPersonal, string> GetQueue()
        {
            var queue = new Dictionary<LeadPersonal, string>();

            foreach (var person in Persons)
            {
                var priority = Priority(person);

                if (priority != null)
                    queue.Add(person, priority);
            }

            return queue.OrderBy(x => x.Value)
                .ThenByDescending(x => x.Key.GetLeadsLeftToGet())
                .ToDictionary(x => x.Key, x => x.Value);
        }

        /// <summary>
        /// return priority value base on initial / number of leads / lead target / lead stop
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        private string Priority(LeadPersonal person)
        {
            if (person.Initial == Config.SuperAccount)
                return Config.SuperAccountPriority;

            if (person.IsOnHold() || person.IsOverLeadStop())
                return null;

            if (person.PriorityToCall)
                return Config.SuperPriority;

            var leadsLeftToGet = LeadLeft(person);
            foreach (var priority in Priorities)
            {
                var roles = (LeadGroups) priority.Role;

                if (roles.Has((LeadGroups) person.GroupId) &&
                    priority.Distance == leadsLeftToGet)
                    return priority.Priority;
            }

            return null;
        }

        /// <summary>
        /// get the lead left to get for the current person
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        private int LeadLeft(LeadPersonal person)
        {
            var limit = Limits.SingleOrDefault(x => ((LeadGroups) x.Role).Has((LeadGroups) person.GroupId));

            var leadsLeftToGet = person.GetLeadsLeftToGet();
            if (leadsLeftToGet > limit.Max)
                return limit.Max.GetValueOrDefault();

            if (leadsLeftToGet < limit.Min)
                return limit.Min.GetValueOrDefault();

            return leadsLeftToGet;
        }
    }
}
