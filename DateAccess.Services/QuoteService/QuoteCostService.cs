using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DataAccess.Common.Util;
using DataAccess.EntityFramework;
using DataAccess.EntityFramework.Models.Quote.Cost;
using DataAccess.EntityFramework.TypeLibrary;
using DateAccess.Services.QuoteService.Helper;

namespace DateAccess.Services.QuoteService
{
    public interface IQuoteCostService : IRepositoryService<Cost>
    {
        Cost CreateUploadCost(Cost cost, FileInfo tempFile,string originalFileName, string path,string user);
        Cost UpdateUploadCost(Cost cost, FileInfo tempFile, string originalFileName, string path);
        Cost CreateSystemCost(Cost cost, string user);
        int Finalize(IList<int> costIds);
        int Delete(IList<int> costIds);
    }

    internal class QuoteCostService : RepositoryService<Cost>, IQuoteCostService
    {
        private readonly IQuoteService _quoteService;
        private readonly ISiteAddressHelper _siteAddressHelper;
        public QuoteCostService(IUnitOfWork unitOfWork,IQuoteService qouteService,ISiteAddressHelper siteAddressHelper) : base(unitOfWork)
        {
            _quoteService = qouteService;
            _siteAddressHelper = siteAddressHelper;
        }
        #region UploadCost
        public Cost CreateUploadCost(Cost cost, FileInfo tempFile, string originalFileName, string path, string user)
        {
            if (cost.PricePa == null || cost.ReturnPw == null) throw new Exception("PA or PW cannot be null");
            CreateCost(cost,user,CostType.Upload);
            var fileName = GetFileName(cost, Path.GetExtension(originalFileName)).FilterInvalidFileName().AppendTimeStamp();
            try
            {
                tempFile.CopyTo(Path.Combine(path, fileName));
                cost.FileName = fileName;
                Save();
            }
            catch (Exception)
            {
                Delete(cost);
                throw;
            }
            finally
            {
                tempFile.Delete();
            }
            return cost;
        }
        //TODO: Need to put entity to cost mapping to Auto-mapper
        public Cost UpdateUploadCost(Cost entity, FileInfo tempFile, string originalFileName, string path)
        {
            if (entity.PricePa == null || entity.ReturnPw == null) throw new Exception("PA or PW cannot be null");
            var cost = GetByKey(entity.Id);
            cost.IsSameAddress = entity.IsSameAddress;
            cost.Address = entity.Address;
            cost.Company = entity.Company;
            cost.Postcode = entity.Postcode;
            cost.State = entity.State;
            cost.PricePa = entity.PricePa;
            cost.ReturnPw = entity.ReturnPw;
            _siteAddressHelper.SameAdressHandlerForCost(cost,cost.Quote);
            _siteAddressHelper.UpdateSiteAddress(cost);
            cost.Quote.LastUpdateDate = DateTime.Now;
            cost.LastUpdateDate = DateTime.Now;
            var fileName = GetFileName(cost, Path.GetExtension(originalFileName)).AppendTimeStamp().FilterInvalidFileName();
            //TODO:Make sure file upload successfully, otherwise we need to delete the cost
            try
            {
                tempFile.CopyTo(Path.Combine(path, fileName));
                cost.FileName = fileName;
                Save();
                //TODO:find more wise way to clean tempsite table
                _siteAddressHelper.RemoveNotUsedTempSite();
            }
            finally
            {
                tempFile.Delete();
            }
            return cost;
        }

      

        #endregion UploadCost

        public Cost CreateSystemCost(Cost cost, string user)
        {
            return CreateCost(cost, user, CostType.System);
        }


        public int Finalize(IList<int> costIds)
        {
            var list = UnitOfWork.QuoteCostRepository.Get(l => costIds.Contains(l.Id));
            foreach (var cost in list)
            {
                cost.Quote.LastUpdateDate = DateTime.Now;
                cost.LastUpdateDate = DateTime.Now;
                if (cost.CostType == CostType.Upload)
                {
                    cost.Status = CostStatus.Finalize;
                }
                if (cost.CostType == CostType.System)
                {
                    //TODO:Add handler to handle system costs
                    throw new NotImplementedException();
                }
            }
            return Save();
        }

        public int Delete(IList<int> costIds)
        {
            var list = UnitOfWork.QuoteCostRepository.Get(l => costIds.Contains(l.Id)).ToArray();
            foreach (var cost in list)
            {
                cost.Quote.LastUpdateDate = DateTime.Now;
            }
            UnitOfWork.QuoteCostRepository.RemoveRange(list);
            return Save();
        }


        private Cost CreateCost(Cost cost,string user,CostType type)
        {
            var quote = UnitOfWork.QuoteRepository.Get(cost.QuoteId);
            if (quote.StatusId == (int) QuoteStatusTypes.New)
            {
                const string description = "Start to estimate costs";
                var nextstatus = _quoteService.NextStatus(quote, true);
                if (nextstatus == null) throw new ArgumentException("Cannot find corresponding next status for " + quote.Status.Name);
                _quoteService.UpdateStatusAndHistory(quote, user, description, (int)nextstatus);
            }
            cost.LastUpdateDate = DateTime.Now;
            cost.CreatedDate = DateTime.Now;
            cost.CostType = type;
            cost.Status = CostStatus.Draft;
            _siteAddressHelper.SameAdressHandlerForCost(cost, quote);
            _siteAddressHelper.UpdateSiteAddress(cost);
            Add(cost);
            //TODO:find more wise way to clean tempsite table
            _siteAddressHelper.RemoveNotUsedTempSite();
            return cost;
        }

        private string GetFileName(Cost cost,string extension)
        {
            string result;
            if (!cost.IsSameAddress)
            {
                result= cost.Id + @"_" + cost.Company + extension;
            }
            else
            {
                var quote = UnitOfWork.QuoteRepository.Get(cost.QuoteId);
                if (quote == null) throw new ArgumentException("Cannot find quote " + cost.QuoteId);
                result = cost.Id + @"_" + quote.Company + extension;
            }
            return result;
        }
    }
}