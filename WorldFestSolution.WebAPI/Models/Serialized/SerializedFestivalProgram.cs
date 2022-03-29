using Newtonsoft.Json;
using System;
using WorldFestSolution.WebAPI.Models.Entities;

namespace WorldFestSolution.WebAPI.Models.Serialized
{
    public class SerializedFestivalProgram
    {
        public SerializedFestivalProgram()
        {
        }

        public SerializedFestivalProgram(FestivalProgram festivalProgram)
        {
            Id = festivalProgram.Id;
            FestivalId = festivalProgram.FestivalId;
            DurationInMinutes = festivalProgram.DurationInMinutes;
            Title = festivalProgram.Title;
            Description = festivalProgram.Description;
            DateTime counter = festivalProgram.Festival.FromDateTime;
            foreach (FestivalProgram program in festivalProgram.Festival.FestivalProgram)
            {
                if (program.Id == festivalProgram.Id)
                {
                    break;
                }
                counter += TimeSpan.FromMinutes(program.DurationInMinutes);
            }
            StartDateTime = counter;
        }

        public int Id { get; set; }
        public int FestivalId { get; set; }
        public int DurationInMinutes { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDateTime { get; set; }
    }
}