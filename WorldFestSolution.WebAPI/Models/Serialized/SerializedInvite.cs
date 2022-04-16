using WorldFestSolution.WebAPI.Models.Entities;

namespace WorldFestSolution.WebAPI.Models.Serialized
{
    public class SerializedInvite
    {
        public SerializedInvite()
        {
        }
        public int Id { get; set; }
        public int OrganizerId { get; set; }
        public int ParticipantId { get; set; }
        public int FestivalId { get; set; }
        public bool IsAccepted { get; set; }
        public bool IsSent { get; set; }
        public SerializedUser User { get; set; }
    }
}