using System;
using System.Linq;
using System.Web;
using WorldFestSolution.WebAPI.Models.Entities;

namespace WorldFestSolution.WebAPI
{
    public static class Identity
    {
        public static int Id
        {
            get
            {
                using (WorldFestBaseEntities entities = new WorldFestBaseEntities())
                {
                    string login = HttpContext.Current.User.Identity.Name;
                    User user = entities.User
                        .First(u => 
                            u.Login.Equals(login, StringComparison.OrdinalIgnoreCase));

                    return user.Id;
                }
            }
        }
    }
}