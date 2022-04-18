using Newtonsoft.Json;
using System.Windows.Input;
using WorldFestSolution.XamarinApp.Models;
using WorldFestSolution.XamarinApp.Models.Serialized;
using WorldFestSolution.XamarinApp.Views;
using Xamarin.Forms;

namespace WorldFestSolution.XamarinApp.ViewModels
{
    public class AccountViewModel : BaseViewModel
    {

        private Command logoutCommand;

        public ICommand LogoutCommand
        {
            get
            {
                if (logoutCommand == null)
                {
                    logoutCommand = new Command(LogoutAsync);
                }

                return logoutCommand;
            }
        }

        internal void OnAppearing()
        {
            IsRefreshing = true;
        }

        private async void LogoutAsync()
        {
            if (await AlertService.Ask("Выйти из аккаунта?"))
            {
                Identity.Logout();
                AppShell.LoadLoginAndRegisterShell();
            }
        }

        private Command refreshCommand;

        public ICommand RefreshCommand
        {
            get
            {
                if (refreshCommand == null)
                {
                    refreshCommand = new Command(RefreshAsync);
                }

                return refreshCommand;
            }
        }

        private ImageSource imageSource;

        private async void RefreshAsync()
        {
            string[] loginAndPassword = CredentialsService.Decode();
            string login = loginAndPassword[0];
            string password = loginAndPassword[1];
            bool isAuthenticated = await AuthenticationService
                .AuthenticateAsync(login, password);
            if (isAuthenticated)
            {
                User responseUser = JsonConvert
                    .DeserializeObject<User>(AuthenticationService.Message);
                ImageSource = responseUser.ImageSource;
                User = responseUser;
            }
            IsRefreshing = false;
        }

        private Command goToChangePasswordViewCommand;

        public ICommand GoToChangePasswordViewCommand
        {
            get
            {
                if (goToChangePasswordViewCommand == null)
                {
                    goToChangePasswordViewCommand = new Command(GoToChangePasswordView);
                }

                return goToChangePasswordViewCommand;
            }
        }

        public ImageSource ImageSource
        {
            get => imageSource;
            set => SetProperty(ref imageSource, value);
        }

        private async void GoToChangePasswordView()
        {
            await Shell.Current.Navigation.PushAsync(
                new ChangePasswordView());
        }

        private Command goToMyInvitesViewCommand;

        public ICommand GoToMyInvitesViewCommand
        {
            get
            {
                if (goToMyInvitesViewCommand == null)
                {
                    goToMyInvitesViewCommand = new Command(GoToMyInvitesViewAsync);
                }

                return goToMyInvitesViewCommand;
            }
        }

        private async void GoToMyInvitesViewAsync()
        {
            await Shell.Current.Navigation.PushAsync(
               new InviteView(
                   new InviteViewModel(festivalId: 0)));
        }
    }
}