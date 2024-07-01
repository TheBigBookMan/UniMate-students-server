using Microsoft.AspNetCore.Mvc;
using UniMate_students_server.Contexts;
using Microsoft.EntityFrameworkCore;
using UniMate_students_server.Models;

namespace UniMate_students_server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DegreesController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DegreesController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private DynamicDbContext GetDynamicDbContext()
        {
            return _httpContextAccessor.HttpContext.Items["DynamicDbContext"] as DynamicDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetDegrees()
        {
            var dbContext = GetDynamicDbContext();
            if(dbContext == null)
            {
                return BadRequest("Database context not found.");
            }

            var degrees = await dbContext.Degrees.ToListAsync();
            return Ok(degrees);
        }

        [HttpPost]
        public async Task<IActionResult> AddDegree([FromBody] Degree degree)
        {
            var dbContext = GetDynamicDbContext();
            if(dbContext == null)
            {
                return BadRequest("Database context not found.");
            }

            dbContext.Degrees.Add(degree);

            try
            {
                var result = await dbContext.SaveChangesAsync();

                if(result > 0)
                {
                    return Ok(degree);
                } else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error creating degree.");
                }

            } catch(Exception ex) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error creating degree: {ex.Message}");
            }
        }

        [HttpGet("{degreeId}")]
        public async Task<IActionResult> GetDegreeById(int degreeId)
        {
            var dbContext = GetDynamicDbContext();
            if(dbContext == null)
            {
                return BadRequest("Database context not found.");
            }

            var degree = await dbContext.Degrees.FindAsync(degreeId);
            if(degree == null)
            {
                return NotFound();
            }

            return Ok(degree);
        }
    }
}
