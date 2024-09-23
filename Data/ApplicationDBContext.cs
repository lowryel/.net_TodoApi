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
        public DbSet<Department> Departments { get; set; } = null!;

        // prevent index being created on the FK
        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Conventions.Remove(typeof(ForeignKeyIndexConvention));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
                .ToTable("Employee")
                .HasOne(e => e.Department)
                .WithMany(e => e.Employees)
                .HasForeignKey(e => e.DepartmentId)
                .IsRequired();

            modelBuilder.Entity<Employee>()
                .Property(e => e.Status)
                .HasColumnType("varchar(32)")
                .HasConversion<string>(v => v.ToString(), v => (EmpStatus)Enum.Parse(typeof(EmpStatus), v));
        }
    }
}

