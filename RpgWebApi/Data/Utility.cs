using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace RpgWebApi.Data
{
    public static class Utility
    {
        public static void CreatePassword(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        public static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            bool result = false;
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                if (Enumerable.SequenceEqual(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)), passwordHash))
                    result = true;
            }

            return result;
        }
    }
}
