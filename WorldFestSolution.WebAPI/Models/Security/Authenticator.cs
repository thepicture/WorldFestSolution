using System;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using WorldFestSolution.WebAPI.Models.Entities;

namespace WorldFestSolution.WebAPI.Models.Security
{
    public static class Authenticator
    {
        public static bool IsAuthenticated(string login,
                                          string password,
                                          out User user)
        {
            using (WorldFestBaseEntities context =
                new WorldFestBaseEntities())
            {
                byte[] passwordBytes = Encoding.UTF8
                    .GetBytes(password);
                user = context.User
                    .Include(u => u.UserType)
                    .FirstOrDefault(
                        u => u.Login
                        .Equals(login, StringComparison.OrdinalIgnoreCase));
                if (user == null)
                {
                    return false;
                }
                byte[] passwordHash = SHA256.Create()
                    .ComputeHash(
                    passwordBytes.Concat(user.Salt)
                        .ToArray());
                return Enumerable.SequenceEqual(user.PasswordHash, passwordHash);
            }
        }
    }
}