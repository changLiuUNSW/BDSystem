using System.Collections.Generic;
using DataAccess.EntityFramework;
using DataAccess.EntityFramework.Models.BD.Allocation;

namespace DateAccess.Services.SiteService
{
    internal class SaleBoxService : ISaleBoxService
    {
         private readonly IUnitOfWork _unitOfWork;

         public SaleBoxService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IList<SalesBox> GetSaleBox(string postCode = null, string state = null)
        {
            return _unitOfWork.SalesBoxRepository.GetSaleBox(postCode, state);
        }
    }
    public interface ISaleBoxService
    {
        IList<SalesBox> GetSaleBox(string postCode = null, string state = null);
    }
}
