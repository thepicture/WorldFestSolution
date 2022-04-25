using WorldFestSolution.WebAPI.Models.Entities;

namespace WorldFestSolution.WebAPI.Models.Serialized
{
    public class SerializedFestivalPopularity
    {
        public SerializedFestivalPopularity(Festival festival)
        {
            FestivalId = festival.Id;
            FestivalTitle = festival.Title;
            CountOfUsers = festival.User.Count;
        }

        public int FestivalId { get; set; }
        public string FestivalTitle { get; set; }
        public int CountOfUsers { get; set; }
    }
}