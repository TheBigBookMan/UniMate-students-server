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

        private UniversityContext GetDynamicDbContext()
        {
            return _httpContextAccessor.HttpContext.Items["DynamicDbContext"] as UniversityContext;
        }

        [HttpPost]
        public async Task<IActionResult> AddDegree([FromBody])
    }
}
