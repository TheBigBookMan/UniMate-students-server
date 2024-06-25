using Microsoft.EntityFrameworkCore;

namespace UniMate_students_server.Contexts
{
    public class StudentContext : DbContext
    {
        public StudentContext(DbContextOptions<StudentContext> options) : base(options) { }

        public DbSet<Student> Students { get; set; }
    }
}
