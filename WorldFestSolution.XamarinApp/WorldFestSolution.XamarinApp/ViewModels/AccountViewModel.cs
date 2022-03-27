using System.Windows.Input;
using WorldFestSolution.XamarinApp.Models;
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

        private async void LogoutAsync()
        {
            if (await AlertService.Ask("Выйти из аккаунта?"))
            {
                Identity.Logout();
                (AppShell.Current as AppShell).LoadLoginAndRegisterShell();
            }
        }
    }
}