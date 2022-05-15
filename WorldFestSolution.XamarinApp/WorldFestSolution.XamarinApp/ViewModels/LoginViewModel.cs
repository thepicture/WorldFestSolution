using System.Windows.Input;
using WorldFestSolution.XamarinApp.Models.Serialized;
using Xamarin.Forms;

namespace WorldFestSolution.XamarinApp.ViewModels
{
    [PropertyChanged.AddINotifyPropertyChangedInterface]
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

        public bool IsCanAuthenticateAsyncExecute => !IsLoginHasError && !IsPasswordHasError;

        private async void AuthenticateAsync(object param)
        {
            IsRefreshing = true;
            if (await LoginUserDataStore.AddItemAsync(LoginUser))
            {
                AppShell.SetShellStacksDependingOnRole();
            }
            IsRefreshing = false;
        }

        public bool IsLoginHasError { get; set; } = true;
        public string LoginErrorText => "Введите логин";
        public bool IsPasswordHasError { get; set; } = true;
        public string PasswordErrorText => "Введите пароль";
    }
}