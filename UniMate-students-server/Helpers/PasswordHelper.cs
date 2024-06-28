using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System;

namespace UniMate_students_server.Helpers
{
    public class PasswordHelper
    {
        public static string CreatePasswordHash(string password, out string salt)
        {
            using (var hmac = new HMACSHA512())
            {
                salt = Convert.ToBase64String(hmac.Key);
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hash);
            }
        }

        public static bool VerifyPasswordHash(string password, string storedHash, string storedSalt)
        {
            using (var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(storedSalt)))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return storedHash == Convert.ToBase64String(computedHash);
            }
        }

        public static string GeneratePassword(string firstName, string lastName)
        {
            string password = (firstName.Length >= 3 ? firstName.Substring(0, 3) : firstName + GetRandomChars(3 - firstName.Length)) +
                              (lastName.Length >= 3 ? lastName.Substring(0, 3) : lastName + GetRandomChars(3 - lastName.Length));
            return password;
        }
    }
}
