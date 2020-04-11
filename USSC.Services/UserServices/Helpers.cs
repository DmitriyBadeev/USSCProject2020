using System.Security.Cryptography;
using System.Text;

namespace USSC.Services.UserServices
{
    public static class Helpers
    {
        public static string ComputeHash(string password)
        {
            var saltString = "USSC_PROFECT_2020";
            var salt = Encoding.UTF8.GetBytes(saltString);
            var algorithm = new SHA256CryptoServiceProvider();

            var bytePassword = Encoding.UTF8.GetBytes(password);

            var saltedPassword = new byte[bytePassword.Length + salt.Length];
            salt.CopyTo(saltedPassword, 0);
            bytePassword.CopyTo(saltedPassword, salt.Length);

            var hashedBytes = algorithm.ComputeHash(saltedPassword);
            var hashedStr = new StringBuilder();
            foreach (var hashedByte in hashedBytes)
                hashedStr.Append(hashedByte.ToString("X2"));

            return hashedStr.ToString();
        }
    }
}
