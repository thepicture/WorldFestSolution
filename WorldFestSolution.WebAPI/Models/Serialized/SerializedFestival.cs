using System;
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
            User organizer = festival.User
                .First(u => u.UserType.Title == "Организатор");
            OrganizerId = organizer.Id;
            OrganizerFullName = $"{organizer.LastName} {organizer.FirstName}";
            UsersId = festival.User
                .Where(u => u.Id != organizer.Id)
                .Select(u => u.Id);
            CommentsId = festival.FestivalComment
               .Select(u => u.Id);
            FestivalProgram = festival.FestivalProgram
                .ToList()
                .ConvertAll(fp => new SerializedFestivalProgram(fp));
            IsActual = System.DateTime.Now < FromDateTime;
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime FromDateTime { get; set; }
        public byte[] Image { get; set; }
        public int CountOfComments { get; set; }
        public int CountOfPrograms { get; set; }
        public int CountOfUsers { get; set; }
        public double Rating { get; set; }
        public int OrganizerId { get; set; }
        public string OrganizerFullName { get; set; }
        public IEnumerable<int> UsersId { get; set; }
        public IEnumerable<int> CommentsId { get; set; }
        public List<SerializedFestivalProgram> FestivalProgram { get; set; }
        public bool IsActual { get; set; }
    }
}