using System.Windows.Input;
using WorldFestSolution.XamarinApp.Models;
using WorldFestSolution.XamarinApp.Models.Serialized;
using Xamarin.Forms;

namespace WorldFestSolution.XamarinApp.ViewModels
{
    public class ChangePasswordViewModel : BaseViewModel
    {

        private Command changePasswordCommand;

        public ICommand ChangePasswordCommand
        {
            get
            {
                if (changePasswordCommand == null)
                {
                    changePasswordCommand = new Command(ChangePasswordAsync);
                }

                return changePasswordCommand;
            }
        }

        private async void ChangePasswordAsync()
        {
            if (await ChangePasswordDataStore.AddItemAsync(Credentials))
            {
                await Shell.Current.GoToAsync("..");
            }
        }

        private string oldPassword;

        public string OldPassword
        {
            get => oldPassword;
            set => SetProperty(ref oldPassword, value);
        }

        private string newPassword;
        private ChangePasswordCredentials credentials;

        public ChangePasswordViewModel()
        {
            Credentials = new ChangePasswordCredentials
            {
                Login = Identity.User.Login,
            };
        }

        public string NewPassword
        {
            get => newPassword;
            set => SetProperty(ref newPassword, value);
        }

        public ChangePasswordCredentials Credentials
        {
            get => credentials;
            set => SetProperty(ref credentials, value);
        }
    }
}