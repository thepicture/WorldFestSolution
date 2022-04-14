using Newtonsoft.Json;
using System;
using System.Windows.Input;
using WorldFestSolution.XamarinApp.Models;
using WorldFestSolution.XamarinApp.Models.Serialized;
using WorldFestSolution.XamarinApp.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WorldFestSolution.XamarinApp.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private string login;

        public string Login
        {
            get => login;
            set => SetProperty(ref login, value);
        }

        private string password;

        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }

        private bool isRememberMe;

        public bool IsRememberMe
        {
            get => isRememberMe;
            set => SetProperty(ref isRememberMe, value);
        }

        private Command authenticateCommand;

        public ICommand AuthenticateCommand
        {
            get
            {
                if (authenticateCommand == null)
                {
                    authenticateCommand = new Command(AuthenticateAsync);
                }

                return authenticateCommand;
            }
        }

        private async void AuthenticateAsync()
        {
            IsBusy = true;
            IsRefreshing = true;
            bool isAuthenticated = await AuthenticationService
                .AuthenticateAsync(Login, Password);
            IsRefreshing = false;
            IsBusy = false;
            if (isAuthenticated)
            {
                string encodedLoginAndPassword =
                    CredentialsToBasicConverter
                    .Encode(Login, Password);
                if (IsRememberMe)
                {
                    Identity.User = JsonConvert
                        .DeserializeObject<User>
                        (AuthenticationService.Message);
                    await SecureStorage
                        .SetAsync("Identity",
                                  encodedLoginAndPassword);
                }
                else
                {
                    (App.Current as App).User = JsonConvert
                        .DeserializeObject<User>
                        (AuthenticationService.Message);
                    (App.Current as App).Role = AuthenticationService.Message;
                    (App.Current as App).Identity = encodedLoginAndPassword;
                }
                await AlertService.Inform("Вы авторизованы " +
                    $"с ролью {Identity.Role}");
                (AppShell.Current as AppShell).SetShellStacksDependingOnRole();
            }
            else
            {
                await AlertService.InformError(AuthenticationService.Message);
            }
            IsBusy = false;
        }

        private Command exitCommand;

        public ICommand ExitCommand
        {
            get
            {
                if (exitCommand == null)
                {
                    exitCommand = new Command(ExitAsync);
                }

                return exitCommand;
            }
        }

        private async void ExitAsync()
        {
            if (await AlertService.Ask("Выключить приложение?"))
            {
                Environment.Exit(0);
            }
        }
    }
}