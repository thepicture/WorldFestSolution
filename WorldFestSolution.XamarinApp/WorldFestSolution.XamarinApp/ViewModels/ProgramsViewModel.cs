using WorldFestSolution.XamarinApp.Models.Serialized;

namespace WorldFestSolution.XamarinApp.ViewModels
{
    public class ProgramsViewModel : BaseViewModel
    {
        private Festival festival;

        public ProgramsViewModel(Festival inputFestival)
        {
            Festival = inputFestival;
        }

        public Festival Festival
        {
            get => festival;
            set => SetProperty(ref festival, value);
        }
    }
}