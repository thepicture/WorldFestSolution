using Xamarin.Forms;

namespace WorldFestSolution.XamarinApp.Models.Serialized
{
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class FestivalPopularity : BindableObject
    {
        public int FestivalId { get; set; }
        public string FestivalTitle { get; set; }
        public int CountOfUsers { get; set; }
    }
}
