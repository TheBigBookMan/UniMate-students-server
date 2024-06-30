using Microsoft.AspNetCore.Mvc;
using UniMate_students_server.Contexts;
using UniMate_students_server.Models;
using Microsoft.EntityFrameworkCore;

namespace UniMate_students_server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProfileController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private UniversityContext GetDynamicDbContext()
        {
            return _httpContextAccessor.HttpContext.Items["DynamicDbContext"] as UniversityContext;
        }

        [HttpGet("{studentId}")]
        public async Task<IActionResult> GetProfile(int StudentId)
        {
            var dbContext = GetDynamicDbContext();
            if(dbContext == null)
            {
                return BadRequest("Database context not found.");
            }

            var student = await dbContext.Students.FindAsync(StudentId);
            if (student == null)
            {
                return NotFound();
            }

            return Ok(student);
        }
    }
}
