using Newtonsoft.Json;
using System;
using System.Windows.Input;
using WorldFestSolution.XamarinApp.Models;
using WorldFestSolution.XamarinApp.Models.Serialized;
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
                    LoginPasswordEncoder
                    .Encode(Login, Password);
                if (IsRememberMe)
                {
                    Identity.User = JsonConvert
                        .DeserializeObject<User>
                        (AuthenticationService.Message);
                    Identity.AuthorizationValue = encodedLoginAndPassword;
                }
                else
                {
                    App.User = JsonConvert
                        .DeserializeObject<User>
                        (AuthenticationService.Message);
                    App.Role = AuthenticationService.Message;
                    App.AuthorizationValue = encodedLoginAndPassword;
                }
                AppShell.SetShellStacksDependingOnRole();
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

        private Command changeApiUrlCommand;

        public ICommand ChangeApiUrlCommand
        {
            get
            {
                if (changeApiUrlCommand == null)
                    changeApiUrlCommand = new Command(ChangeApiUrlAsync);

                return changeApiUrlCommand;
            }
        }

        private async void ChangeApiUrlAsync()
        {
            string newBaseUrl = await App.Current.MainPage
                   .DisplayPromptAsync("Изменить путь к API",
                                       "Введите путь к API",
                                       initialValue: Api.BaseUrl);
            Models.Api.BaseUrl = newBaseUrl;
        }
    }
}