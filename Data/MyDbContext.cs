using EmployeeManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> users { get; set; }
        public DbSet<Employee> employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Email unique ho
            modelBuilder.Entity<User>()
                .HasIndex(u => u.email)
                .IsUnique();

            // EmployeeCode unique ho
            modelBuilder.Entity<Employee>()
                .HasIndex(e => e.EmployeeCode)
                .IsUnique();

            // User (1) -> Employee (0 or 1)
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.User)
                .WithOne(u => u.Employee)
                .HasForeignKey<Employee>(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}