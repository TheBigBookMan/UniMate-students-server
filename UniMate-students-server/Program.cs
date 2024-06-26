using Microsoft.EntityFrameworkCore;
using UniMate_students_server.Contexts;
using UniMate_students_server.Factories;
using UniMate_students_server.Middlewares;

namespace UniMate_students_server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<CentralDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("CentralDatabase")));

            builder.Services.AddScoped<DynamicDbContextFactory>();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.UseMiddleware<DynamicDatabaseMiddleware>();

            app.MapControllers();
            app.Run();
        }
    }
}