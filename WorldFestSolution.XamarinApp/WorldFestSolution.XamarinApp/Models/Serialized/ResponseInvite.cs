using Newtonsoft.Json;
using Xamarin.Forms;

namespace WorldFestSolution.XamarinApp.Models.Serialized
{
    public class ResponseInvite
    {
        public int Id { get; set; }
        public int OrganizerId { get; set; }
        public int ParticipantId { get; set; }
        public int FestivalId { get; set; }
        public bool IsAccepted { get; set; }
        public bool IsSent { get; set; }
        public User Organizer { get; set; }
        public User Participant { get; set; }
        public string FestivalTitle { get; set; }
        [JsonIgnore]
        public string VisibleFullName => Organizer?.FullName
            ?? Participant.FullName;
        [JsonIgnore]
        public ImageSource VisibleImageSource => Organizer?.ImageSource
            ?? Participant.ImageSource;
    }
}
