using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataAccess.EntityFramework;
using DataAccess.EntityFramework.Infrastructure;
using Moq;

namespace DataAccess.Services.UnitTests
{
    internal class MockHelper
    {
        public MockHelper()
        {
            Init();
        }

        public virtual Mock<IUnitOfWork> UnitOfWork { get; private set; }

        private void Init()
        {
            UnitOfWork = new Mock<IUnitOfWork>();
        }

        public Mock<IRepository<T>> CreateMock<T>() where T : class
        {
            return new Mock<IRepository<T>>();
        }

        public Mock<IRepository<T>> Setup<T>(IQueryable<T> query) where T:class 
        {
            /*var mock = new Mock<IDbSet<T>>();
            var m = new Mock<IRepository<T>>();

            mock.Setup(x => x.Provider).Returns(query.Provider);
            mock.Setup(x => x.Expression).Returns(query.Expression);
            mock.Setup(x => x.ElementType).Returns(query.ElementType);
            mock.Setup(x => x.GetEnumerator()).Returns(query.GetEnumerator());*/

            throw new NotImplementedException();
        }
    }
}
