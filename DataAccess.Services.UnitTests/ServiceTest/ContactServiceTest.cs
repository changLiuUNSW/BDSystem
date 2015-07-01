using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DataAccess.EntityFramework.Infrastructure;
using DataAccess.EntityFramework.Models.BD.Allocation;
using DataAccess.EntityFramework.Models.BD.Contact;
using DataAccess.EntityFramework.Repositories;
using DateAccess.Services.ContactService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DataAccess.Services.UnitTests.ServiceTest
{
    internal struct GmGroup
    {
        public int LeadStop { get; set; }
        public int LeadTarget { get; set; }
    }

    [TestClass]
    public class ContactServiceTest : TestClass
    {
        private IContactService _contactService;

            
        [TestInitialize]
        public override void Init()
        {
            base.Init();
        }


        /// <summary>
        /// verify when there is no lead priority record available, no contact will be retrived for calling
        /// </summary>
        [TestMethod]
        public void NextContact_Expect_NoPriorityAvailable_ContactIsNull()
        {
        }

        /// <summary>
        /// verify when there is no allocations been assigned, no contact will be retrieved for calling
        /// </summary>
        [TestMethod]
        public void NextContact_Expect_HasNoAllocation_ContactIsNull()
        {
        }

        [TestCleanup]
        public void Cleanup()
        {
        }

        public void Setup(IList<Allocation> allocations)
        {
            var mockAllocation = new Mock<IRepository<Allocation>>();
            mockAllocation.Setup(x => x.Include(It.IsAny<Expression<Func<Allocation, object>>[]>())).Returns(allocations);
            MockHelper.UnitOfWork.Setup(x => x.AllocationRepository).Returns(mockAllocation.Object);

            var mockContacts = new Mock<IContactRepository>();
            //mockContacts.Setup(x=>x.NextCleaningContact(It.IsAny<IList<string>>(), It.IsAny<IList<Allocation>>())).Returns(new Contact());
        }
    }
}
