using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess.Common;
using DataAccess.EntityFramework;
using DataAccess.EntityFramework.Infrastructure;
using DataAccess.EntityFramework.Models.BD.Lead;
using DateAccess.Services.ContactService;
using DateAccess.Services.MailService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DataAccess.Services.UnitTests.ServiceTest
{
    [TestClass]
    public class LeadServiceTest
    {
        private Mock<ApplicationSettings>  _settings;
        private Mock<IEmailHelper> _emailHelper;
        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<LeadService> _leadService;

        [TestMethod]
        public async Task GetAllLeadStatusAsyncTest()
        {
            var repo = new Mock<IRepository<LeadStatus>>();
            var testList = new List<LeadStatus>
            {
                new LeadStatus
                {
                    Id =1
                }
            };
            repo.Setup(l => l.GetAsync()).ReturnsAsync(testList);
            _unitOfWork.Setup(l => l.GetRepository<LeadStatus>()).Returns(repo.Object);
            var result= await _leadService.Object.GetAllLeadStatus();
            Assert.AreEqual(testList, result);
        }

       



        [TestInitialize]
        public void Init()
        {
            _settings=new Mock<ApplicationSettings>();
            _emailHelper=new Mock<IEmailHelper>();
            _unitOfWork=new Mock<IUnitOfWork>();
            _leadService = new Mock<LeadService>(_unitOfWork.Object, _settings.Object, _emailHelper.Object);
        }
    }
}
