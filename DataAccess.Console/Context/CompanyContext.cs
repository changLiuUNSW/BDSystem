using System.Data.Entity;
using DataAccess.Console.Models;

namespace DataAccess.Console.Context
{
    public partial class CompanyContext : DbContext
    {
        public CompanyContext()
            : base("name=CompanyContext")
        {
        }

        public virtual DbSet<COMPTEMP> COMPTEMP { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
