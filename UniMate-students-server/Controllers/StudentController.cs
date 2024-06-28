using Microsoft.AspNetCore.Mvc;
using UniMate_students_server.Contexts;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using UniMate_students_server.Models;

namespace UniMate_students_server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public StudentsController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private UniversityContext GetDynamicDbContext()
        {
            return _httpContextAccessor.HttpContext.Items["DynamicDbContext"] as UniversityContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetStudents()
        {
            var dbContext = GetDynamicDbContext();
            if (dbContext == null)
            {
                return BadRequest("Database context not found.");
            }

            var students = await dbContext.Students.ToListAsync();
            return Ok(students);
        }

        [HttpPost]
        public async Task<IActionResult> AddStudent(Student student)
        {
            var dbContext = GetDynamicDbContext();
            if (dbContext == null)
            {
                return BadRequest("Database context not found.");
            }

            student.Dob = DateTime.SpecifyKind(student.Dob, DateTimeKind.Utc);

            dbContext.Students.Add(student);
            await dbContext.SaveChangesAsync();

            // TODO this will need to also set the password hash to some sort of defaul or something

            return Ok(student);
        }
    }
}