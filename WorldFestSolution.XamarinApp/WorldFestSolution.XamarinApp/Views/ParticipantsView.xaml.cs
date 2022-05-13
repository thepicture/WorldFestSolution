using RatingBarControl;
using WorldFestSolution.XamarinApp.Models.Serialized;
using WorldFestSolution.XamarinApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WorldFestSolution.XamarinApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ParticipantsView : ContentPage
    {
        private readonly ParticipantsViewModel _viewModel;

        public ParticipantsView(ParticipantsViewModel participantsViewModel)
        {
            InitializeComponent();
            BindingContext = _viewModel = participantsViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}