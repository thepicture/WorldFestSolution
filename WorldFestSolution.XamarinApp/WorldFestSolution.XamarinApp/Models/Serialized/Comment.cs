namespace WorldFestSolution.XamarinApp.Models.Serialized
{
    public class Comment
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Text { get; set; }
        public System.DateTime CreationDateTime { get; set; }
        public int FestivalId { get; set; }
        public string UserFullName { get; set; }
    }
}