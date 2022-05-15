using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace WorldFestSolution.WebAPI.Models.Security
{
    public static class PasswordHashGenerator
    {
        public static byte[] Encrypt(string plainPassword, byte[] salt)
        {
            List<byte> passwordBytesAndHash = Encoding.UTF8
                .GetBytes(plainPassword)
                .ToList();
            passwordBytesAndHash.AddRange(salt);
            byte[] passwordHash = SHA256.Create()
                .ComputeHash(
                passwordBytesAndHash.ToArray());
            return passwordHash;
        }
    }
}