using WorldFestSolution.XamarinApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WorldFestSolution.XamarinApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AccountView : ContentPage
    {
        private readonly AccountViewModel _viewModel;

        public AccountView() : this(new AccountViewModel()) { }

        public AccountView(AccountViewModel accountViewModel)
        {
            InitializeComponent();
            MessagingCenter
              .Subscribe<AccountViewModel, double>(this,
                                                    nameof(UpdateRatingBar),
                                                    UpdateRatingBar);
            BindingContext = _viewModel = accountViewModel;
        }

        private void UpdateRatingBar(AccountViewModel _, double value)
        {
            if (value == 0)
            {
                AccountRatingBar.star1.Source = AccountRatingBar.EmptyStarImage;
                AccountRatingBar.star2.Source = AccountRatingBar.EmptyStarImage;
                AccountRatingBar.star3.Source = AccountRatingBar.EmptyStarImage;
                AccountRatingBar.star4.Source = AccountRatingBar.EmptyStarImage;
                AccountRatingBar.star5.Source = AccountRatingBar.EmptyStarImage;
            }
            else
            {
                AccountRatingBar.SelectedStarValue = (int)value;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}