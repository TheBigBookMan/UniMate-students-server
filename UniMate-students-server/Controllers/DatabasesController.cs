using Microsoft.AspNetCore.Mvc;
using UniMate_students_server.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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
    }
}