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

            var university = await dbContext.University.FirstOrDefaultAsync(u => u.UniversityName == universityName);
            if(university == null)
            {
                return NotFound("University not found");
            }

            var db_id = university.db_id;

            var bucketName = "unimates-profile-pictures";
            var key = $"{db_id}/{Guid.NewGuid()}_{file.FileName}";

            try
            {
                using (var newMemoryStream = new MemoryStream())
                {
                    file.CopyTo(newMemoryStream);
                    var uploadRequest = new TransferUtilityUploadRequest
                    {
                        InputStream = newMemoryStream,
                        Key = key,
                        BucketName = bucketName,
                        ContentType = file.ContentType,
                        CannedACL = S3CannedACL.PublicRead
                    };

                    var fileTransferUtility = new TransferUtility(_s3Client);
                    await fileTransferUtility.UploadAsync(uploadRequest);

                    var fileUrl = $"https://{bucketName}.s3.amazonaws.com/{key}";

                    var student = await dbContext.Students.FindAsync(formData.StudentId);
                    if(student == null)
                    {
                        return NotFound("Student not found.");
                    }

                    student.ProfilePic = fileUrl;
                    dbContext.Students.Update(student);
                    await dbContext.SaveChangesAsync();

                    return Ok(new { Url = fileUrl });
                }

            } catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        public class FileRequest
        {
            public IFormFile File { get; set; }
            public int StudentId { get; set; }
        }
    }
}
