using System.Windows.Input;
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
    }
}