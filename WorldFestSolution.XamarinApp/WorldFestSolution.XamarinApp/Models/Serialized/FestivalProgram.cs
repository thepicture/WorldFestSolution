using System;

namespace WorldFestSolution.XamarinApp.Models.Serialized
{
    public class FestivalProgram
    {
        public int Id { get; set; }
        public int FestivalId { get; set; }
        public int DurationInMinutes { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDateTime { get; set; }
    }
}