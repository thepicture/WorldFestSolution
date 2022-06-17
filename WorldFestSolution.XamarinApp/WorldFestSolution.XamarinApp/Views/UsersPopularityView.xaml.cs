using WorldFestSolution.XamarinApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WorldFestSolution.XamarinApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UsersPopularityView : ContentPage
    {
        private readonly UsersPopularityViewModel _viewModel;

        public UsersPopularityView()
        {
            InitializeComponent();
            BindingContext = _viewModel = new UsersPopularityViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}