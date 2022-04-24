namespace WorldFestSolution.WebAPI.Models.Serialized
{
    public class SerializedUserOfFestival
    {
        public int FestivalId { get; set; }
        public bool IsParticipating { get; set; }
        public bool IsWantsToChangeState { get; set; }
    }
}