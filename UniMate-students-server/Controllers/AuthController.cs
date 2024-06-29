﻿using Microsoft.AspNetCore.Mvc;
using UniMate_students_server.Contexts;
using Microsoft.EntityFrameworkCore;
using UniMate_students_server.Models;
using Microsoft.AspNetCore.Identity.Data;
using System.Text;
using System.Security.Cryptography;
using UniMate_students_server.Helpers;
using UniMate_students_server.DTOs.AuthDTO;

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

        // This not working, fix later
        [HttpPut("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] int StudentId)
        {
            var dbContext = GetDynamicDbContext();
            if(dbContext == null)
            {
                return BadRequest("Database context not found.");
            }

            var authRecord = await dbContext.Auth
                .FirstOrDefaultAsync(a => a.StudentId == StudentId);

            if(authRecord == null)
            {
                return NotFound("Student not found.");
            }

            authRecord.PasswordHash = "";
            authRecord.PasswordSalt = "";

            try
            {
                await dbContext.SaveChangesAsync();
                return Ok("Password reset successful.");

            } catch(DbUpdateException ex)
            {
                var innerException = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error resetting password: {innerException}");

            } catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error resetting password: {ex.Message}");
            }
        }

        // TODO this will need to work in unison with frontend where a new user being added has a password hash of their uni name plus username or something and then 
        // if the password hash is empty then return for set new password

        // TODO create a reset password path

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var dbContext = GetDynamicDbContext();
            if (dbContext == null)
            {
                return BadRequest("Database context not found.");
            }

            var student = await dbContext.Students.FirstOrDefaultAsync(s => s.Username == request.username);
            if(student == null)
            {
                return NotFound();
            }

            var authRecord = await dbContext.Auth.FirstOrDefaultAsync(a => a.StudentId == student.StudentId);

            if (authRecord == null)
            {
                return Unauthorized("Inavlid credentials, cannot find user");
            }

            if(string.IsNullOrEmpty(authRecord.PasswordHash))
            {
                var resetPasswordResponse = new BaseResponse
                {
                    StudentId = student.StudentId,
                    ResponseMessage = "reset password"
                };
                return Ok(resetPasswordResponse);
            }

            if(!PasswordHelper.VerifyPasswordHash(request.password, authRecord.PasswordHash, authRecord.PasswordSalt))
            {
                return Unauthorized("Invalid credentials");
            }

            
        }

        public class LoginRequest
        {
            public string username { get; set; }
            public string password { get; set; }
        }
    }
}
