using Newtonsoft.Json;
using System.Windows.Input;
using WorldFestSolution.XamarinApp.Models;
using WorldFestSolution.XamarinApp.Models.Serialized;
using WorldFestSolution.XamarinApp.Services;
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
                (AppShell.Current as AppShell).LoadLoginAndRegisterShell();
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

        private async void RefreshAsync()
        {
            string[] loginAndPassword = BasicToCredentialsConverter.Decode();
            string login = loginAndPassword[0];
            string password = loginAndPassword[1];
            bool isAuthenticated = await AuthenticationService
                .AuthenticateAsync(login, password);
            if (isAuthenticated)
            {
                User = JsonConvert
                    .DeserializeObject<User>(AuthenticationService.Message);
            }
            IsRefreshing = false;
        }
    }
}