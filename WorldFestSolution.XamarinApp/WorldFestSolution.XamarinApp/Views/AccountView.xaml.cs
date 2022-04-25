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
            BindingContext = _viewModel = accountViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}