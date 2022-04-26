using Newtonsoft.Json;

namespace WorldFestSolution.XamarinApp.Models.Serialized
{
    public class FestivalRating
    {
        public int FestivalId { get; set; }
        public int CountOfStars { get; set; }
        [JsonIgnore]
        public bool IsRated { get; set; }
    }
}
