using Microsoft.EntityFrameworkCore;

namespace UniMate_students_server.Models
{
    public class CentralDbContext : DbContext
    {
        public CentralDbContext(DbContextOptions<CentralDbContext> options) : base(options) { }

        public DbSet<University> Universities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<University>()
                .HasKey(u => u.UniversityId);

            base.OnModelCreating(modelBuilder);
        }
    }
}