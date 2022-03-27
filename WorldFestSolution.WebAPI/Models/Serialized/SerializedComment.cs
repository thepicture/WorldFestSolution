using WorldFestSolution.WebAPI.Models.Entities;

namespace WorldFestSolution.WebAPI.Models.Serialized
{
    public class SerializedComment
    {
        public SerializedComment()
        {
        }

        public SerializedComment(FestivalComment festivalComment)
        {
            Id = festivalComment.Id;
            UserId = festivalComment.UserId;
            Text = festivalComment.Text;
            CreationDateTime = festivalComment.CreationDateTime;
            FestivalId = festivalComment.Id;
            UserFullName = festivalComment.User.LastName
                + festivalComment.User.FirstName;
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public string Text { get; set; }
        public System.DateTime CreationDateTime { get; set; }
        public int FestivalId { get; set; }
        public string UserFullName { get; set; }
    }
}