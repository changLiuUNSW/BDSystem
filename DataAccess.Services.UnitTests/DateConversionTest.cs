using System;
using DataAccess.EntityFramework.Extensions.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataAccess.Services.UnitTests
{
    [TestClass]
    public class DateConversionTest
    {
        [TestInitialize]
        public void Init()
        {
        }

        [TestMethod]
        public void TestToday_ExpectIsThisWeek()
        {
            var isThisWeek = DateTime.Today.Date.IsThisWeek();
            Assert.IsTrue(isThisWeek);
        }

        [TestMethod]
        public void TestToday_ExpectNotLastWeek()
        {
            var isLastWeek = DateTime.Today.Date.IsLastWeek();
            Assert.IsFalse(isLastWeek);
        }

        [TestMethod]
        public void TestLastWeek_ExpectNotThisWeek()
        {
            var isThisWeek = LastWeek().Date.IsThisWeek();
            Assert.IsFalse(isThisWeek);
        }

        [TestMethod]
        public void TestLastWeek_ExpectIsLastWeek()
        {
            var isLastWeek = LastWeek().Date.IsLastWeek();
            Assert.IsTrue(isLastWeek);
        }

        private DateTime NextWeek()
        {
            var daysToNextWeek = 8 - (int)DateTime.Today.DayOfWeek;
            return DateTime.Today.AddDays(daysToNextWeek);
        }

        private DateTime LastWeek()
        {
            var daysToLastWeek = 0 - (int) DateTime.Today.DayOfWeek;
            return DateTime.Today.AddDays(daysToLastWeek);
        }
    }
}
