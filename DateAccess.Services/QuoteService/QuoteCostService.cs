using System;
using System.Collections.Generic;
using System.IO;
using DataAccess.Common.Util;
using DataAccess.EntityFramework;
using DataAccess.EntityFramework.Models.BD;
using DataAccess.EntityFramework.Models.Quote.Cost;
using DataAccess.EntityFramework.TypeLibrary;

namespace DateAccess.Services.QuoteService
{
    public interface IQuoteCostService : IRepositoryService<Cost>
    {
        Cost CreateUploadCost(Cost cost, FileInfo tempFile,string originalFileName, string path,string user);
        Cost UpdateUploadCost(Cost cost, FileInfo tempFile, string originalFileName, string path);
        int Finalize(IList<int> costIds);
        int Delete(IList<int> costIds);
    }

    internal class QuoteCostService : RepositoryService<Cost>, IQuoteCostService
    {
        private readonly IQuoteService _quoteService;  
        public QuoteCostService(IUnitOfWork unitOfWork,IQuoteService qouteService) : base(unitOfWork)
        {
            _quoteService = qouteService;
        }

        public Cost CreateUploadCost(Cost cost, FileInfo tempFile, string originalFileName, string path, string user)
        {
            var quote = UnitOfWork.QuoteRepository.Get(cost.QuoteId);
            if (cost.PricePa == null || cost.ReturnPw == null) throw new Exception("PA or PW cannot be null");
            if (cost.IsSameAddress)cost.Address=new Address();
            
            if (quote.StatusId == (int) QuoteStatusTypes.New)
            {
                const string description = "Start to estimate costs";
                var nextstatus = _quoteService.NextStatus(quote, true);
                if (nextstatus == null) throw new ArgumentException("Cannot find corresponding next status for " + quote.Status.Name);
                _quoteService.UpdateStatusAndHistory(quote, user, description, (int)nextstatus);
            }
            cost.LastUpdateDate = DateTime.Now;
            cost.CreatedDate = DateTime.Now;
            cost.CostType=CostType.Upload;
            cost.Status = CostStatus.Draft;
            Add(cost);
            Save();
            var fileName = GetFileName(cost, Path.GetExtension(originalFileName)).FilterInvalidFileName().AppendTimeStamp();
            //TODO:Make sure file upload successfully, otherwise we need to delete the cost
            try
            {
                tempFile.CopyTo(Path.Combine(path, fileName));
                cost.FileName = fileName;
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
            Save();
            return cost;
        }

        public Cost UpdateUploadCost(Cost cost, FileInfo tempFile, string originalFileName, string path)
        {
            var quote = UnitOfWork.QuoteRepository.Get(cost.QuoteId);
            quote.LastUpdateDate = DateTime.Now;
            cost.LastUpdateDate = DateTime.Now;
            Update(cost);
            Save();
            var fileName = GetFileName(cost, Path.GetExtension(originalFileName)).AppendTimeStamp().FilterInvalidFileName();
            //TODO:Make sure file upload successfully, otherwise we need to delete the cost
            try
            {
                tempFile.CopyTo(Path.Combine(path, fileName));
                cost.FileName = fileName;
                Save();
            }
            finally
            {
                tempFile.Delete();
            }
            return cost;
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
            var list = UnitOfWork.QuoteCostRepository.Get(l => costIds.Contains(l.Id));
            foreach (var cost in list)
            {
                cost.Quote.LastUpdateDate = DateTime.Now;
            }
            UnitOfWork.QuoteCostRepository.RemoveRange(list);
            return Save();
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