using Newtonsoft.Json;

namespace WorldFestSolution.XamarinApp.Models.Serialized
{
    public class UserRating
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int? RaterId { get; set; }
        public int CountOfStars { get; set; }
        [JsonIgnore]
        public bool IsRated { get; set; }
    }
}
