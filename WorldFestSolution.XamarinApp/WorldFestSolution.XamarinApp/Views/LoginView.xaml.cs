using WorldFestSolution.XamarinApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WorldFestSolution.XamarinApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginView : ContentPage
    {
        public LoginView()
        {
            InitializeComponent();
        }

        private void OnLoginChanged(object sender, TextChangedEventArgs e)
        {
            LoginViewModel viewModel = BindingContext as LoginViewModel;
            viewModel.IsLoginHasError = string.IsNullOrWhiteSpace(viewModel.LoginUser.Login);
        }

        private void OnPasswordChanged(object sender, TextChangedEventArgs e)
        {
            LoginViewModel viewModel = BindingContext as LoginViewModel;
            viewModel.IsPasswordHasError = string.IsNullOrWhiteSpace(viewModel.LoginUser.Password);
        }
    }
}