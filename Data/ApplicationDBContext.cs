using Microsoft.EntityFrameworkCore;

namespace EmployeeAdminPortal.Data
{
    public class ApplicationDBContext : DbContext
    {
        // works with multiple DBContext by stating the names in the arrow brackets
        public ApplicationDBContext(DbContextOptions options) : base(options)
        { 
        }
        public DbSet<Employee> Employees { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().ToTable("Employee"); 
        }
    }
}