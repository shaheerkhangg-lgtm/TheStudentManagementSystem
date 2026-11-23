using Microsoft.EntityFrameworkCore;
using TheStudentManagementSystem.Models;

namespace TheStudentManagementSystem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<ExamResult> ExamResults { get; set; }
        public DbSet<AttendanceRecord> AttendanceRecords { get; set; }
        public DbSet<Fee> Fees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Two users cannot have the same email
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Two teachers cannot have the same email
            modelBuilder.Entity<Teacher>()
                .HasIndex(t => t.Email)
                .IsUnique();

            // Existing and new users start as Pending (NEW)
            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasDefaultValue("Pending");
        }
    }
}