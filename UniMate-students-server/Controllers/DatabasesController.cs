using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UniMate_students_server.Contexts;
using UniMate_students_server.Models;

namespace UniMate_students_server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DatabasesController : ControllerBase
    {
        private readonly CentralDbContext _context;

        public DatabasesController(CentralDbContext context)
        {
            _context = context;
        }

        [HttpGet("universities")]
        public async Task<IActionResult> GetUniversities()
        {
            var universities = await _context.Universities.ToListAsync();
            return Ok(universities);
        }

        [HttpPost("universities")]
        public async Task<IActionResult> AddUniversity([FromBody] University university)
        {
            if (university == null) 
            {
                return BadRequest("University is null");
            }

            university.DatabaseHash = Guid.NewGuid().ToString();

            _context.Universities.Add(university);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUniversities), new { id = university.UniversityId }, university);
        }
    }
}