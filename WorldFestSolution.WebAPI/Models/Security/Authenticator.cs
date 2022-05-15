using System;
using System.Data.Entity;
using System.Linq;
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
                user = context.User
                    .Include(u => u.UserType)
                    .FirstOrDefault(
                        u => u.Login
                        .Equals(login, StringComparison.OrdinalIgnoreCase));
                if (user == null)
                {
                    return false;
                }
                byte[] passwordHash = PasswordHashGenerator.Encrypt(password, user.Salt);
                return Enumerable.SequenceEqual(user.PasswordHash, passwordHash);
            }
        }
    }
}