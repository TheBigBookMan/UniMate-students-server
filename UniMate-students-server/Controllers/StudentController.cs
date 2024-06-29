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
                    string password = PasswordHelper.GeneratePassword(student.FirstName, student.LastName);
                    Console.WriteLine(password);
                    var auth = new Auth
                    {
                        StudentId = student.StudentId,
                        PasswordHash = PasswordHelper.CreatePasswordHash(password, out string salt),
                        PasswordSalt = salt,
                        CreatedAt = DateTime.UtcNow
                    };

                    Console.WriteLine(auth);
                    dbContext.Auths.Add(auth);

                    var authResult = await dbContext.SaveChangesAsync();
                    if(authResult > 0)
                    {
                        return CreatedAtAction(nameof(GetStudentById), new { id = student.StudentId }, student);
                    } else
                    {
                        // If the Auth creation fails, rollback the Student creation
                        dbContext.Students.Remove(student);
                        await dbContext.SaveChangesAsync();
                        return StatusCode(StatusCodes.Status500InternalServerError, "Error creating authentication record.");
                    }

                } else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error creating Student");
                }

            } catch (DbUpdateException ex)
            {
                // Log the inner exception details for more information
                var innerException = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error creating student: {innerException}");

            } catch (Exception ex) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error creating student: {ex.Message}");
            }

        }

        [HttpGet("{studentId}")]
        public async Task<IActionResult> GetStudentById(int studentId)
        {
            var dbContext = GetDynamicDbContext();
            if(dbContext ==  null)
            {
                return BadRequest("Database context not found.");
            }

            var student = await dbContext.Students.FindAsync(studentId);
            if(student == null)
            {
                return NotFound();
            }

            return Ok(student);
        }
    }
}