using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using UniMate_students_server.Contexts;
using UniMate_students_server.Factories;

namespace UniMate_students_server.Middlewares
{
    public class DynamicDatabaseMiddleware
    {
        private readonly RequestDelegate _next;

        public DynamicDatabaseMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var universityName = context.Request.Headers["UniversityName"].ToString();

            if (string.IsNullOrEmpty(universityName))
            {
                universityName = context.Request.Cookies["UniversityName"];
            }

            if (!string.IsNullOrEmpty(universityName))
            {
                using (var scope = context.RequestServices.CreateScope())
                {
                    var centralDbContext = scope.ServiceProvider.GetRequiredService<CentralDbContext>();

                    var dbContextFactory = scope.ServiceProvider.GetRequiredService<DynamicDbContextFactory>();

                    var university = await centralDbContext.Universities
                        .FirstOrDefaultAsync(u => u.UniversityName == universityName);

                    if (university != null)
                    {
                        var dbContext = dbContextFactory.CreateDbContext(university.db_id);
                        context.Items["DynamicDbContext"] = dbContext;
                    }
                }
            }

            await _next(context);
        }
    }
}