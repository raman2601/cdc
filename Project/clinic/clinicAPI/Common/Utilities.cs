using clinicAPI.Data.Model;
using Microsoft.AspNetCore.Identity;
using System.Reflection.Metadata.Ecma335;

namespace clinicAPI.Common
{
    public static class Utilities
    {
        public static string PasswordHash(User user)
        {
            var passwordHasher = new PasswordHasher<object>();

            // Hash the password
            return passwordHasher.HashPassword(user, user.PasswordHash);

            // Verify the password
           
        }
        public static string PasswordHash(User user,string password)
        {
            var passwordHasher = new PasswordHasher<object>();

            // Hash the password
            return passwordHasher.HashPassword(user, password);

            // Verify the password

        }
        public static Boolean VerifyPassword(User user, String password)
        {
            var passwordHasher = new PasswordHasher<object>();
            var verificationResult = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
            if( verificationResult == PasswordVerificationResult.Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
