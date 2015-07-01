using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataAccess.Services.UnitTests
{
    [TestClass]
    public class TestClass
    {
        internal MockHelper MockHelper { get; set; }

        [TestInitialize]
        public virtual void Init()
        {
            MockHelper = new MockHelper();
        }
    }
}
