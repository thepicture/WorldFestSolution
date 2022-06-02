using System.Linq;
using System.Web;
using WorldFestSolution.WebAPI.Models.Entities;

namespace WorldFestSolution.WebAPI.Models.Serialized
{
    public class SerializedUser
    {
        private const int DefaultRating = 0;

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
            Is18OrMoreYearsOld = user.Is18OrMoreYearsOld;
            IsWantsInvites = user.IsWantsInvites;
            if (user.UserRating.Count > 0)
            {
                Rating = user.UserRating.Average(ur => ur.CountOfStars);
            }
            else
            {
                Rating = DefaultRating;
            }
            IsRated = user.UserRating.Any(r =>
            {
                return r.User1?.Login == HttpContext.Current.User.Identity.Name;
            });
            CountOfFestivals = user.Festival.Count;
            CountOfComments = user.FestivalComment.Count;
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
        public bool IsRated { get; set; }
        public bool? Is18OrMoreYearsOld { get; set; }
        public bool IsWantsInvites { get; set; }
        public int CountOfFestivals { get; set; }
        public int CountOfComments { get; set; }
    }
}