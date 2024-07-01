using Microsoft.AspNetCore.Mvc;
using UniMate_students_server.Contexts;
using UniMate_students_server.Models;
using Microsoft.EntityFrameworkCore;
using Amazon.S3;
using Amazon.S3.Transfer;

namespace UniMate_students_server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAmazonS3 _s3Client;

        public ProfileController(IHttpContextAccessor httpContextAccessor, IAmazonS3 s3Client)
        {
            _httpContextAccessor = httpContextAccessor;
            _s3Client = s3Client;
        }

        private DynamicDbContext GetDynamicDbContext()
        {
            return _httpContextAccessor.HttpContext.Items["DynamicDbContext"] as DynamicDbContext;
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

        [HttpPost("upload-profile")]
        public async Task<IActionResult> AddProfilePic([FromBody] FileRequest formData)
        {
            var dbContext = GetDynamicDbContext();
            if(dbContext == null)
            {
                return BadRequest("Database context not found.");
            }

            var file = formData.File;
            

            if(file == null || file.Length == 0)
            {
                return BadRequest("File is empty or not provided.");
            }

            var universityName = Request.Cookies["UniversityName"];
            if(string.IsNullOrEmpty(universityName))
            {
                return BadRequest("University name not found");
            }

            var university = await dbContext.Universi

            var bucketName = "s3://unimates-profile-pictures/test1/";
        }

        private class FileRequest
        {
            public IFormFile File { get; set; }
            public int StudentId { get; set; }
    }
}
