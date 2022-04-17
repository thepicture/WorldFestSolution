namespace WorldFestSolution.XamarinApp.Models.Serialized
{
    public class RequestInvite
    {
        public int Id { get; set; }
        public int OrganizerId { get; set; }
        public int ParticipantId { get; set; }
        public int FestivalId { get; set; }
        public bool IsAccepted { get; set; }

    }
}
