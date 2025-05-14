using System.Security.Cryptography;
using System.Text;

namespace WerkzeugMobil.Helpers
{
    public static class PasswordGenerator
    {
        public static string Generate(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            using var rng = RandomNumberGenerator.Create();
            var bytes = new byte[length];
            rng.GetBytes(bytes);
            var result = new StringBuilder(length);
            foreach (var b in bytes)
                result.Append(chars[b % chars.Length]);
            return result.ToString();
        }
    }
}
