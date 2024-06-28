using Microsoft.AspNetCore.Mvc;
using UniMate_students_server.Contexts;
using Microsoft.EntityFrameworkCore;
using UniMate_students_server.Models;
using Microsoft.AspNetCore.Identity.Data;
using System.Text;
using System.Security.Cryptography;

namespace UniMate_students_server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private UniversityContext GetDynamicDbContext()
        {
            return _httpContextAccessor.HttpContext.Items["DynamicDbContext"] as UniversityContext;
        }

        // TODO add in a signup endpoint and this can set the password hash
        // TODO this will need to work in unison with frontend where a new user being added has a password hash of their uni name plus username or something and then 

        // TODO create a reset password path

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var dbContext = GetDynamicDbContext();
            if (dbContext == null)
            {
                return BadRequest("Database context not found.");
            }
            
            var authRecord = await dbContext.Auths.FirstOrDefaultAsync(a => a.StudentId == request.StudentId);

            if (authRecord == null)
            {
                return Unauthorized("Inavlid credentials");
            }

            if(!VerifyPasswordHash(request.Password, authRecord.PasswordHash, authRecord.PasswordSalt))
            {
                return Unauthorized("Invalid credentials");
            }

            return Ok("Login successful.");
        }

        private bool VerifyPasswordHash(string password, string storedHash, string storedSalt)
        {
            using (var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(storedSalt)))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return storedHash == Convert.ToBase64String(computedHash);
            }
        }

        public class LoginRequest
        {
            public int StudentId { get; set; }
            public string Password { get; set; }
        }
    }
}
