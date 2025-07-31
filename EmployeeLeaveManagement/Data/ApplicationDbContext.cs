using Microsoft.EntityFrameworkCore;
using EmployeeLeaveManagement.Models;

namespace EmployeeLeaveManagement.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }
        public DbSet<LeaveBalance> LeaveBalances { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure User entity
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            });

            // Configure LeaveRequest entity
            modelBuilder.Entity<LeaveRequest>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Reason).IsRequired().HasMaxLength(500);
                entity.Property(e => e.ManagerComments).HasMaxLength(500);
                
                entity.HasOne(e => e.Employee)
                    .WithMany(u => u.LeaveRequests)
                    .HasForeignKey(e => e.EmployeeId)
                    .OnDelete(DeleteBehavior.Restrict);
                
                entity.HasOne(e => e.Manager)
                    .WithMany(u => u.ManagedLeaveRequests)
                    .HasForeignKey(e => e.ManagerId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure LeaveBalance entity
            modelBuilder.Entity<LeaveBalance>(entity =>
            {
                entity.HasKey(e => e.Id);
                
                entity.HasOne(e => e.User)
                    .WithMany(u => u.LeaveBalances)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                // Ensure unique combination of User, LeaveType, and Year
                entity.HasIndex(e => new { e.UserId, e.LeaveType, e.Year }).IsUnique();
            });

            // Seed initial data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed default manager
            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 1,
                Name = "Admin Manager",
                Email = "admin@company.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                Role = UserRole.Manager,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            });

            // Seed sample employee
            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 2,
                Name = "John Doe",
                Email = "john.doe@company.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("employee123"),
                Role = UserRole.Employee,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            });

            // Seed leave balances for sample employee
            modelBuilder.Entity<LeaveBalance>().HasData(
                new LeaveBalance
                {
                    Id = 1,
                    UserId = 2,
                    LeaveType = LeaveType.Vacation,
                    TotalDays = 20,
                    UsedDays = 0,
                    Year = DateTime.Now.Year,
                    LastUpdated = DateTime.UtcNow
                },
                new LeaveBalance
                {
                    Id = 2,
                    UserId = 2,
                    LeaveType = LeaveType.SickLeave,
                    TotalDays = 10,
                    UsedDays = 0,
                    Year = DateTime.Now.Year,
                    LastUpdated = DateTime.UtcNow
                }
            );
        }
    }
} 