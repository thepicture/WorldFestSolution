using WorldFestSolution.XamarinApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WorldFestSolution.XamarinApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserChatView : ContentPage
    {
        private UserChatViewModel _viewModel;

        public UserChatView()
        {
            InitializeComponent();
        }

        public UserChatView(UserChatViewModel userChatViewModel)
        {
            InitializeComponent();
            BindingContext = _viewModel = userChatViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}