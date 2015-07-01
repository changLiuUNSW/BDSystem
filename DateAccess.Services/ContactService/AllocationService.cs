using System;
using System.Collections.Generic;
using System.Linq;
using DataAccess.EntityFramework;
using DataAccess.EntityFramework.Models.BD.Allocation;

namespace DateAccess.Services.ContactService
{
    public interface IAllocationService : IRepositoryService<Allocation>
    {
        IList<ZoneSize> IdleZones();
    }

    public struct ZoneSize
    {
        public string Zone { get; set; }
        public string Size { get; set; }
    }

    internal class AllocationService : RepositoryService<Allocation>, IAllocationService
    {
        public AllocationService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public override Allocation Add(Allocation item)
        {
            var allocations = UnitOfWork.AllocationRepository.Get(x => x.Initial == item.Initial);

            if (allocations.Any(x=>x.Zone == item.Zone && x.Size == item.Size))
                throw new Exception("Allocation already exist");

            return base.Add(item);
        }

        public IList<ZoneSize> IdleZones()
        {
            var allocations = UnitOfWork.AllocationRepository.Distinct(x => new
            {
                size = x.Size,
                zone = x.Zone
            }).Select(x => new ZoneSize
            {
                Size = x.size,
                Zone = x.zone
            });

            var zones = UnitOfWork.SalesBoxRepository.Distinct(x => x.Zone);
            var zoneWithSize = new List<ZoneSize>();
            var sizes = new[] {"008", "025", "050", "120"};
            foreach (var zone in zones)
            {
                zoneWithSize.AddRange(sizes.Select(size => new ZoneSize
                {
                    Zone = zone, Size = size
                }));
            }
            return zoneWithSize.Where(x => !allocations.Contains(x)).ToList();
        }
    }
}
