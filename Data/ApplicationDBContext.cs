using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace EmployeeAdminPortal.Data
{
    public class ApplicationDBContext : DbContext
    {
        // works with multiple DBContext by stating the names in the arrow brackets
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        { 
        }
        public DbSet<Employee> Employees { get; set; } = null!;
        public DbSet<Accountant> Accountants { get; set; } = null!;

        // prevent index being created on the FK
        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Conventions.Remove(typeof(ForeignKeyIndexConvention));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
                .ToTable("Employee");
                
            modelBuilder.Entity<Accountant>()
                .ToTable("Accountant");
        }
    }
}