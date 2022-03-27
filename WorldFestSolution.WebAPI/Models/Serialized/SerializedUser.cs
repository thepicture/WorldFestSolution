using System.Linq;
using WorldFestSolution.WebAPI.Models.Entities;

namespace WorldFestSolution.WebAPI.Models.Serialized
{
    public class SerializedUser
    {
        public SerializedUser()
        {
        }

        public SerializedUser(User user)
        {
            Id = user.Id;
            Login = user.Login;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Patronymic = user.Patronymic;
            UserTypeId = user.UserTypeId;
            Image = user.Image;
            if (user.UserRating.Count > 0)
            {
                Rating = user.UserRating.Average(ur => ur.CountOfStars);
            }
            else
            {
                Rating = 0;
            }
        }

        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public int UserTypeId { get; set; }
        public byte[] Image { get; set; }
        public double Rating { get; set; }
    }
}