namespace WorldFestSolution.WebAPI.Models.Serialized
{
    public class SerializedInvite
    {
        public int Id { get; set; }
        public int OrganizerId { get; set; }
        public int ParticipantId { get; set; }
        public int FestivalId { get; set; }
        public bool IsAccepted { get; set; }
        public bool IsSent { get; set; }
        public string FestivalTitle { get; set; }
        public bool IsParticipantWantsInvites { get; set; }
        public SerializedUser Organizer { get; set; }
        public SerializedUser Participant { get; set; }
        public SerializedFestival Festival { get; set; }
    }
}