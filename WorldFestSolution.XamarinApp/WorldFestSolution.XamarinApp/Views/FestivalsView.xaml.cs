using WorldFestSolution.XamarinApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WorldFestSolution.XamarinApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FestivalsView : ContentPage
    {
        private readonly FestivalsViewModel _viewModel;

        public FestivalsView(FestivalsViewModel festivalsViewModel)
        {
            InitializeComponent();
            BindingContext = _viewModel = festivalsViewModel;
            FilterPicker.SelectedIndex = 0;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}