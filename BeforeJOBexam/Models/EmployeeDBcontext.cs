using Microsoft.EntityFrameworkCore;

namespace BeforeJOBexam.Models
{
    public class EmployeeDBContext : DbContext
    {
        public EmployeeDBContext(DbContextOptions<EmployeeDBContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Optional: table configuration
            modelBuilder.Entity<Employee>()
                .ToTable("Employees");
        }
    }
}