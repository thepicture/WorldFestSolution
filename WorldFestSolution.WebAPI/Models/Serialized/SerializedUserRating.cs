using WorldFestSolution.WebAPI.Models.Entities;

namespace WorldFestSolution.WebAPI.Models.Serialized
{
    public class SerializedUserRating
    {
        public SerializedUserRating(UserRating rating)
        {
            Id = rating.Id;
            UserId = rating.UserId;
            RaterId = rating.RaterId;
            CountOfStars = rating.CountOfStars;
        }
        public int Id { get; set; }
        public int UserId { get; set; }
        public int? RaterId { get; set; }
        public int CountOfStars { get; set; }
    }
}