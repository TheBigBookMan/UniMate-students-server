using Microsoft.EntityFrameworkCore;
using UniMate_students_server.Models;

namespace UniMate_students_server.Contexts
{
    public class UniversityContext : DbContext
    {
        public UniversityContext(DbContextOptions<UniversityContext> options) : base(options) { }

        public DbSet<Student> Students { get; set; }
        public DbSet<Auth> Auth { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define the relationship between Student and Auth
            modelBuilder.Entity<Auth>()
                .HasOne(a => a.Student)
                .WithMany()
                .HasForeignKey(a => a.StudentId);
        }
    }
}
