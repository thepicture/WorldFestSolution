using WorldFestSolution.XamarinApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WorldFestSolution.XamarinApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FestivalsPopularityChartView : ContentPage
    {
        private readonly FestivalsPopularityChartViewModel _viewModel;

        public FestivalsPopularityChartView()
        {
            InitializeComponent();
            BindingContext = _viewModel = new FestivalsPopularityChartViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}