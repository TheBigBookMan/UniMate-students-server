using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UniMate_students_server.Contexts;

namespace UniMate_students_server.Factories
{
    public class DynamicDbContextFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public DynamicDbContextFactory(IServiceProvider serviceProvider) 
        { 
            _serviceProvider = serviceProvider;
        }

        public StudentContext CreateDbContext(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<StudentContext>();

            // TODO this wont work because the DBID returns and will need to be in the conention string
            optionsBuilder.UseNpgsql(connectionString);

            return new StudentContext(optionsBuilder.Options);
        }
    }
}
