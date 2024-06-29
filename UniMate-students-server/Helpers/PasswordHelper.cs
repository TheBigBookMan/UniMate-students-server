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
            var saltBytes = Convert.FromBase64String(storedSalt);
            using (var hmac = new HMACSHA512(saltBytes))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return storedHash == Convert.ToBase64String(computedHash);
            }
        }

        public static string GeneratePassword(string firstName, string lastName)
        {
            string password = (firstName.Length >= 3 ? firstName.Substring(0, 3) : firstName + GetRandomChars(3 - firstName.Length)) +
                              (lastName.Length >= 3 ? lastName.Substring(0, 3) : lastName + GetRandomChars(3 - lastName.Length));
            Console.WriteLine(password);
            return password;
        }

        private static string GetRandomChars(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var result = new char[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = chars[random.Next(chars.Length)];
            }
            return new string(result);
        }
    }
}
