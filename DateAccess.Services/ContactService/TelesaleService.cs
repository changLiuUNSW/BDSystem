using System;
using DataAccess.EntityFramework;
using DataAccess.EntityFramework.Models.BD.Telesale;

namespace DateAccess.Services.ContactService
{
    public interface ITelesaleService : IRepositoryService<Telesale>
    {
        void AddAssignment(int id, Assignment assignment);
        void RemoveAssignment(int id, string code, string area);
    }

    internal class TelesaleService : RepositoryService<Telesale>, ITelesaleService
    {
        public TelesaleService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            
        }

        public void AddAssignment(int id, Assignment assignment)
        {
            if (UnitOfWork.TelesaleAssignmentRepository.Any(x => x.QpCode == assignment.QpCode &&
                                                                 x.Area == assignment.Area &&
                                                                 x.Size == assignment.Size))
                throw new Exception("The area has been assigned, refresh the page to see.");

            base.GetByKey(id).Assignments.Add(assignment);
            UnitOfWork.Save();
        }

        public void RemoveAssignment(int id, string code, string area)
        {
            UnitOfWork.TelesaleAssignmentRepository.Delete(id, code, area);
            UnitOfWork.Save();
        }
    }
}
