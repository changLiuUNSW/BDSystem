using System;
using System.Collections.Generic;
using System.Linq;
using DataAccess.Common.Paging;
using DataAccess.EntityFramework;
using DataAccess.EntityFramework.Extensions.Utilities;
using DataAccess.EntityFramework.Models.BD.Allocation;
using DataAccess.EntityFramework.TypeLibrary;
using DateAccess.Services.ViewModels;

namespace DateAccess.Services.ContactService
{
    public interface IAreaService : IRepositoryService<SalesBox>
    {
        IList<SalesBox> Get(string postcode, string state, int? take);
        Paging<SalesBox> GetPage(int page, int size, string postcode);
        IList<ZoneAllocation> ZoneAllocations { get; }
        IList<string> States { get; }
        IList<string> Zones { get; } 
    }

    internal class AreaService : RepositoryService<SalesBox>, IAreaService
    {
        public AreaService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
           
        }

        public IList<string> States
        {
            get { return Distinct(x => x.State).Where(x=>!string.IsNullOrEmpty(x)).ToList(); }
        }

        public IList<string> Zones
        {
            get { return Distinct(x => x.Zone); }
        }

        public IList<ZoneAllocation> ZoneAllocations
        {
            get
            {
                UnitOfWork.EnableProxyCreation(false);
                var zones = UnitOfWork.SalesBoxRepository.Distinct(x => x.Zone);
                var allocations = UnitOfWork.AllocationRepository.Get();
                UnitOfWork.EnableProxyCreation(true);
                return zones.Select(x => new ZoneAllocation
                {
                    Zone = x,
                    Allocations = allocations.Where(y => y.Zone == x)
                }).ToList();
            }
        }

        /// <summary>
        /// get salesbox base on postcode and state
        /// </summary>
        /// <param name="postcode"></param>
        /// <param name="state"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public IList<SalesBox> Get(string postcode, string state, int? take)
        {
            return UnitOfWork.SalesBoxRepository.GetSaleBox(postcode, state, take);
        }

        public Paging<SalesBox> GetPage(int page, int size, string postcode)
        {
            if (string.IsNullOrEmpty(postcode))
                return UnitOfWork.SalesBoxRepository.Page(page, size);

            return UnitOfWork.SalesBoxRepository.Page(page, size, postcode);
        }
    }
}
