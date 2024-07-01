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

        [HttpGet("databases")]
        public async Task<IActionResult> GetDatabases()
        {
            var databases = await _context.Databases.ToListAsync();
            return Ok(databases);
        }

        [HttpPost("databases")]
        public async Task<IActionResult> AddDatabase([FromBody] Database database)
        {
            if (database == null) 
            {
                return BadRequest("Databases is null");
            }

            database.DatabaseHash = Guid.NewGuid().ToString();

            _context.Databases.Add(database);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDatabases), new { id = database.DatabaseId }, database);
        }
    }
}