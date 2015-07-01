using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using DataAccess.EntityFramework.Models.BD.Lead;
using DateAccess.Services.ContactService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DataAccess.Services.UnitTests.ServiceTest
{
    [TestClass]
    public class ContactPriorityTest : TestClass
    {
        private readonly LeadGroup _gm = new LeadGroup { LeadStop = 4, LeadTarget = 1, Group = "GM"};
        private readonly LeadGroup _ggm = new LeadGroup { LeadStop = 4, LeadTarget = 1, Group = "GGM" };
        private readonly LeadGroup _rop = new LeadGroup { LeadStop = 4, LeadTarget = 3, Group = "ROP" };
        private readonly LeadGroup _bd = new LeadGroup { LeadStop = 14, LeadTarget = 5, Group = "BD" };

        [TestInitialize]
        public override void Init()
        {
            base.Init();
        }

        /// <summary>
        /// verify the call queue is in correct order
        /// </summary>
        [TestMethod]
        public void TestPriority_ExpectPrioritySortCorrectly()
        {
            //setup lead priority
            /*var expect = new LeadPersonal {GroupId = _gm.Group, LeadGroup = _gm, Initial = "expect"};
            var bd = new LeadPersonal {GroupId = _bd.Group, LeadGroup = _bd, Initial = _bd.Group};

            //setup
            var gmGroup = new LeadPriority
            {
                Distance = _gm.LeadTarget,
                Priority = "01",
                Role = _gm.Group
            };

            var bdGroup = new LeadPriority
            {
                Distance = _bd.LeadTarget,
                Priority = "02",
                Role = _bd.Group
            };

            Setup(new List<LeadPriority> {gmGroup, bdGroup}, new List<LeadPersonal> {expect, bd});

            var actual = _contactPriority.Priority;
            Assert.AreEqual(expect.Initial, actual.First().Key);*/
        }

        /// <summary>
        /// verify that leads are used to determine the queue when priority is the same
        /// </summary>
        [TestMethod]
        public void TestPriority_ExpectQueueOrderByLeadDescending()
        {
            //one person in gm group
            /*var gm = new LeadPersonal { GMGroup = _gm.Group, LeadGroup = _gm, Initial = _gm.Group, Leads = {new Lead()}};
            var expect = new LeadPersonal { GMGroup = _gm.Group, LeadGroup = _gm, Initial = "expect" };

            //one priority group in bd group
            var gmGroup = new LeadPriority
            {
                Distance = _gm.LeadTarget - gm.Leads.Count,
                Priority = "01",
                Role = _gm.Group
            };

            //one priority group in bd group
            var expectGroup = new LeadPriority
            {
                Distance = _gm.LeadTarget,
                Priority = "01",
                Role = _gm.Group
            };

            Setup(new List<LeadPriority> {expectGroup, gmGroup}, new List<LeadPerson> {expect, gm});

            var actual = _contactPriority.Priority;
            Assert.AreEqual(expect.Initial, actual.First().Key);*/
        }

        /// <summary>
        /// verify when priority to call is set, target is always on top of the queue
        /// </summary>
        [TestMethod]
        public void TestPriority_ExpectPriorityToCallOnTopOfQueue()
        {
            //one person in gm group
            /*var expect = new LeadPerson { GMGroup = _bd.Group, LeadGroup = _bd, Initial = "expect", PriorityToCall = true};
            var gm = new LeadPerson { GMGroup = _gm.Group, LeadGroup = _gm, Initial = _gm.Group};

            var gmGroup = new LeadPriority
            {
                Distance = _gm.LeadTarget,
                Priority = "02",
                Role = _gm.Group
            };

            Setup(new List<LeadPriority> {gmGroup}, new List<LeadPerson> {expect, gm});

            var actual = _contactPriority.Priority;
            Assert.AreEqual(expect.Initial, actual.First().Key);*/
        }

        /// <summary>
        /// verify that if the target has hit the lead stop, he will be excluded from the queue
        /// </summary>
        [TestMethod]
        public void TestPriority_ExpectOverLeadStopExcludeFromQueue()
        {
            //one person in gm group
            /*var expect = new LeadPerson { GMGroup = _gm.Group, LeadGroup = _gm, Initial = "expect", PriorityToCall = true, Leads = new Collection<Lead>()};

            for (var i = 0; i <= _gm.LeadStop; i++)
            {
                expect.Leads.Add(new Lead());
            }

            var gmGroup = new LeadPriority
            {
                Distance = _gm.LeadTarget - expect.Leads.Count,
                Priority = "01",
                Role = _gm.Group
            };

            Setup(new List<LeadPriority> {gmGroup}, new List<LeadPerson> {expect});

            var actual = _contactPriority.Priority;
            Assert.AreEqual(0, actual.Count);*/
        }

        /// <summary>
        /// verify that if the target is put on hold, he will be excluded from the queue
        /// </summary>
        [TestMethod]
        public void TestPriority_ExpectLeadNoHoldExcludeFromQueue()
        {
            /*var expect =
                new LeadPersonal
                {
                    GMGroup = _gm.Group,
                    LeadGroup = _gm,
                    Initial = "expect",
                    PriorityToCall = true,
                    LeadsOnHoldDate = DateTime.Today.AddDays(1)
                };

            var gmGroup = new LeadPriority
            {
                Distance = _gm.LeadTarget,
                Priority = "01",
                Role = _gm.Group
            };

            Setup(new List<LeadPriority> {gmGroup}, new List<LeadPerson> {expect});

            var actual = _contactPriority.Priority;
            Assert.AreEqual(0, actual.Count);*/
        }

        /// <summary>
        /// verify that lead target property will take precedence over default lead target in acquiring the priority
        /// </summary>
        [TestMethod]
        public void TestPriority_LeadTargetTakePrecedenceOverDefault()
        {
            /*const int leadTarget = 10;
            var expect = new LeadPerson {GMGroup = _gm.Group, LeadGroup = _gm, Initial = "expect", LeadTarget = leadTarget};

            var gmGroup = new LeadPriority
            {
                Distance = leadTarget,
                Priority = "01",
                Role = _gm.Group
            };

            Setup(new List<LeadPriority> {gmGroup}, new List<LeadPersonal> {expect});

            var actual = _contactPriority.Priority;
            Assert.AreEqual(expect.Initial, actual.First().Key);*/
        }

        /// <summary>
        /// verify that lead stop property will take precedence over default value 
        /// </summary>
        [TestMethod]
        public void TestPriority_LeadStopTakePrecedenceOverDefault()
        {
            /*const int leadStop = 10;
            var expect = new LeadPersonal{LeadGroup = _gm.Group, LeadGroup = _gm, Initial = "expect", LeadStop = leadStop, Leads = new Collection<Lead>()};

            for (var i = 0; i <= leadStop; i++)
            {
                expect.Leads.Add(new Lead());
            }

            var gmGroup = new LeadPriority
            {
                Distance = _gm.LeadTarget,
                Priority = "01",
                Role = _gm.Group
            };

            Setup(new List<LeadPriority> {gmGroup}, new List<LeadPersonal> {expect});

            var actual = _contactPriority.Priority;
            Assert.AreEqual(0, actual.Count);*/
        }

        private void Setup(IEnumerable<LeadPriority> expectedLeadGroups, IList<LeadPersonal> expectedLeadQpPriorities)
        {
            LeadPriority expectedGroup = null;
            var mockLeadGroup = MockHelper.CreateMock<LeadPriority>();

            mockLeadGroup.Setup(
                x => x.SingleOrDefault(It.IsAny<Expression<Func<LeadPriority, bool>>>()))
                .Callback<Expression<Func<LeadPriority, bool>>>(x => expectedGroup = expectedLeadGroups.SingleOrDefault(x.Compile()))
                .Returns(() => expectedGroup);

            MockHelper.UnitOfWork.Setup(x => x.PriorityGroupRepository).Returns(mockLeadGroup.Object);

            var mock = MockHelper.CreateMock<LeadPersonal>();
            mock.Setup(x => x.Get()).Returns(expectedLeadQpPriorities);
            //MockHelper.UnitOfWork.Setup(x => x.LeadPersonalRepository).Returns(mock.Object);
        }
    }
}
