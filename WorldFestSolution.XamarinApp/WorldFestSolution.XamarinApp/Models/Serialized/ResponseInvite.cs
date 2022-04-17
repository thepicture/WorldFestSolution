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
        public User User { get; set; }
    }
}
