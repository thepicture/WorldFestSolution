using WorldFestSolution.XamarinApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WorldFestSolution.XamarinApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FestivalView : ContentPage
    {
        private readonly FestivalViewModel _viewModel;

        public FestivalView(FestivalViewModel festivalViewModel)
        {
            InitializeComponent();
            MessagingCenter
                .Subscribe<FestivalViewModel, double>(this,
                                                      nameof(UpdateRatingBar),
                                                      UpdateRatingBar);
            BindingContext = _viewModel = festivalViewModel;
        }

        private void UpdateRatingBar(FestivalViewModel _, double value)
        {
            if (FestivalRatingBar.SelectedStarValue != 0 && value == 0)
            {
                FestivalRatingBar.star1.Source = FestivalRatingBar.EmptyStarImage;
                FestivalRatingBar.star2.Source = FestivalRatingBar.EmptyStarImage;
                FestivalRatingBar.star3.Source = FestivalRatingBar.EmptyStarImage;
                FestivalRatingBar.star4.Source = FestivalRatingBar.EmptyStarImage;
                FestivalRatingBar.star5.Source = FestivalRatingBar.EmptyStarImage;
            }
            else
            {
                FestivalRatingBar.SelectedStarValue = (int)value;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}