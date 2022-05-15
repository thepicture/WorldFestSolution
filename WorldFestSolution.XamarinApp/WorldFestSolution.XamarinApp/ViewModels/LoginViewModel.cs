using System;
using System.Windows.Input;
using WorldFestSolution.XamarinApp.Models;
using WorldFestSolution.XamarinApp.Models.Serialized;
using Xamarin.Forms;

namespace WorldFestSolution.XamarinApp.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public LoginUser LoginUser { get; set; } = new LoginUser();

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
            IsRefreshing = true;
            if (await LoginUserDataStore.AddItemAsync(LoginUser))
            {
                AppShell.SetShellStacksDependingOnRole();
            }
            IsRefreshing = false;
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