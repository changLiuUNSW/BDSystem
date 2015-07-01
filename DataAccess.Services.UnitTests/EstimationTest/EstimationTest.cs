using System;
using DataAccess.EntityFramework.Models.Quote.Cost;
using DateAccess.Services.QuoteService.Calculator;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataAccess.Services.UnitTests.EstimationTest
{
    [TestClass]
    public class EstimationTest
    {
        [TestMethod]
        public void TestDummy()
        {
            var cost = new Cost
            {
                RegCleanWeeks = 52,
                RegSubcontractorCostPw = 500
            };

            var price = SmallQuoteEstimator.Australia.GetCost(cost);
            Console.WriteLine("Price is {0}", price.SubTotal);
            Assert.AreEqual(0, price.Total);
        }
    }
}
