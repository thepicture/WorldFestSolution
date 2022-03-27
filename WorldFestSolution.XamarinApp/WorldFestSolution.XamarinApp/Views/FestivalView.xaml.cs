using WorldFestSolution.XamarinApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WorldFestSolution.XamarinApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FestivalView : ContentPage
    {
        public FestivalView()
        {
            InitializeComponent();
        }

        public FestivalView(FestivalViewModel festivalViewModel)
        {
            InitializeComponent();
            BindingContext = festivalViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            (BindingContext as FestivalViewModel).OnAppearing();
        }
    }
}