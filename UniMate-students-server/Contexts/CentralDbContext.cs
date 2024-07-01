using Microsoft.EntityFrameworkCore;
using UniMate_students_server.Models;

namespace UniMate_students_server.Contexts
{
    public class CentralDbContext : DbContext
    {
        public CentralDbContext(DbContextOptions<CentralDbContext> options) : base(options) { }

        public DbSet<Database> Databases { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Database>()
                .HasKey(u => u.DatabaseId);

            base.OnModelCreating(modelBuilder);
        }
    }
}