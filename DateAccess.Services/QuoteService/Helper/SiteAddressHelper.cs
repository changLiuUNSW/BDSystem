using DataAccess.EntityFramework;
using DataAccess.EntityFramework.Models.BD.Site;
using DataAccess.EntityFramework.Models.Quote;
using DataAccess.EntityFramework.Models.Quote.Cost;

namespace DateAccess.Services.QuoteService.Helper
{
    public interface ISiteAddressHelper
    {
        void UpdateSiteAddress(QuoteBase entity);
        void SameAdressHandlerForCost(Cost cost,Quote quote);
        int RemoveNotUsedTempSite();
    }
    internal class SiteAddressHelper : ISiteAddressHelper
    {
        private readonly IUnitOfWork _unitOfWork;

        public SiteAddressHelper(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //TODO: Need to find more wise way to handle tempsite
        public void UpdateSiteAddress(QuoteBase entity)
        {
            var site=_unitOfWork.SiteRepository.CheckSiteExists(entity.Company,
                entity.Address.Unit, 
                entity.Address.Number,
                entity.Address.Street, 
                entity.Address.Suburb, 
                entity.Postcode, 
                entity.State);
            if (site != null)
            {
                entity.TempSiteId = null;
                entity.SiteId = site.Id;
            }
            else
            {
                entity.SiteId = null;
                //check existing site info in temsite table
                var tempsite = _unitOfWork.TempSiteRepository.CheckTempSiteExists(
                    entity.Company,
                    entity.Address.Unit,
                    entity.Address.Number,
                    entity.Address.Street,
                    entity.Address.Suburb,
                    entity.Postcode,
                    entity.State
                    );
                if (tempsite != null)
                {
                    entity.TempSiteId = tempsite.Id;
                }
                else
                {
                    entity.TempSite = new TempSite
                    {
                        Name = entity.Company,
                        Address = entity.Address,
                        State = entity.State,
                        Postcode = entity.Postcode,
                    };
                }
            }
        }

        public void SameAdressHandlerForCost(Cost cost, Quote quote)
        {
            if (!cost.IsSameAddress) return;
            cost.Address = quote.Address;
            cost.Company = quote.Company;
            cost.State = quote.State;
            cost.Postcode = quote.Postcode;
        }

        public int RemoveNotUsedTempSite()
        {
            return _unitOfWork.TempSiteRepository.RemoveNotUsedTempSite();
        }
    }
}
