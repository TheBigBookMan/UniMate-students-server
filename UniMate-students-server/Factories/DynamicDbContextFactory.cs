using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UniMate_students_server.Contexts;
using Microsoft.Extensions.Configuration;

namespace UniMate_students_server.Factories
{
    public class DynamicDbContextFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;

        public DynamicDbContextFactory(IServiceProvider serviceProvider, IConfiguration configuration) 
        { 
            _serviceProvider = serviceProvider;
            _configuration = configuration;
        }

        public StudentContext CreateDbContext(string dbId)
        {
            var connectionStringTemplate = _configuration.GetConnectionString("UniversityDatabaseTemplate");
            var connectionString = connectionStringTemplate.Replace("{db_id}", dbId);

            var optionsBuilder = new DbContextOptionsBuilder<StudentContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new StudentContext(optionsBuilder.Options);
        }
    }
}
