using Microsoft.AspNetCore.Mvc;
using UniMate_students_server.Contexts;
using Microsoft.EntityFrameworkCore;
using UniMate_students_server.Models;
using UniMate_students_server.Helpers;

namespace UniMate_students_server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UniversityController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UniversityController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private DynamicDbContext GetDynamicDbContext()
        {
            return _httpContextAccessor.HttpContext.Items["DynamicDbContext"] as DynamicDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetUniversity()
        {
            var dbContext = GetDynamicDbContext();
            if(dbContext == null)
            {
                return BadRequest("Database context not found.");
            }

            var university = await dbContext.University.ToListAsync();
            return Ok(university);
        }

    }
}
