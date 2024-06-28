using Microsoft.AspNetCore.Mvc;
using UniMate_students_server.Contexts;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using UniMate_students_server.Models;
using UniMate_students_server.Helpers;

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

            try
            {
                var result = await dbContext.SaveChangesAsync();
                if(result > 0)
                {
                    var auth = new Auth
                    {
                        StudentId = student.StudentId,
                        PasswordHash = CreatePassword()
                    }

                } else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error creating Student");
                }

            } catch (Exception ex) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error creating student: {ex.Message}");
            }

            // TODO this will create an entry but a temporaryu password



            return Ok(student);
        }
    }
}