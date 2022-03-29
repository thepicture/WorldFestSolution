using System.Collections.Generic;
using System.Linq;
using WorldFestSolution.WebAPI.Models.Entities;

namespace WorldFestSolution.WebAPI.Models.Serialized
{
    public class SerializedFestival
    {
        public SerializedFestival()
        {
        }

        public SerializedFestival(Festival festival)
        {
            Id = festival.Id;
            Title = festival.Title;
            Description = festival.Description;
            FromDateTime = festival.FromDateTime;
            Image = festival.Image;
            CountOfComments = festival.FestivalComment.Count;
            CountOfPrograms = festival.FestivalProgram.Count;
            CountOfUsers = festival.User.Count;
            if (festival.FestivalRating.Count > 0)
            {
                Rating = festival.FestivalRating
                    .Average(fr => fr.CountOfStars);
            }
            else
            {
                Rating = 0;
            }
            Organizer = new SerializedUser(
                festival.User.First());
            IsActual = System.DateTime.Now < FromDateTime;
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public System.DateTime FromDateTime { get; set; }
        public byte[] Image { get; set; }
        public int CountOfComments { get; set; }
        public int CountOfPrograms { get; set; }
        public int CountOfUsers { get; set; }
        public double Rating { get; set; }
        public SerializedUser Organizer { get; set; }
        public bool IsActual { get; set; }

    }
}