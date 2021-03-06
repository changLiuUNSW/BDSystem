﻿using System;
using System.Diagnostics;
using System.Linq;
using DataAccess.EntityFramework;
using DataAccess.EntityFramework.DbContexts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataAccess.Services.UnitTests.ServiceTest
{
    [TestClass]
    public class ReportTest
    {
        [TestMethod]
        public void ExcuteTimeTest()
        {

            using (var uow = new UnitOfWork(new SiteResourceEntities()))
            {
                var watcher = Stopwatch.StartNew();
                var person = uow.LeadPersonalRepository.Filter(x => x.Allocations.Any(), x => x, x=>x.Allocations).First();
                var test = uow.ContactRepository.AllocationSummary();
                watcher.Stop();
                Console.WriteLine("Time taken for whole process {0}", watcher.Elapsed);
                Console.WriteLine("end");
            }
        }
    }
}
