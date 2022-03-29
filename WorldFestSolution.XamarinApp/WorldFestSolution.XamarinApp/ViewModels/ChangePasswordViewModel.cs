using System.Text;
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
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(OldPassword))
            {
                _ = errors.AppendLine("Введите старый пароль");
            }
            if (string.IsNullOrWhiteSpace(NewPassword))
            {
                _ = errors.AppendLine("Введите новый пароль");
            }
            if (!string.IsNullOrWhiteSpace(OldPassword)
                && !string.IsNullOrWhiteSpace(NewPassword)
                && OldPassword == NewPassword)
            {
                _ = errors.AppendLine("Новый пароль " +
                    "должен отличаться от старого пароля");
            }
            if (errors.Length > 0)
            {
                await AlertService.InformError(
                    errors.ToString());
                return;
            }

            ChangePasswordCredentials credentials = new ChangePasswordCredentials
            {
                Login = Identity.User.Login,
                OldPassword = OldPassword,
                NewPassword = NewPassword
            };
            if (await ChangePasswordDataStore.AddItemAsync(credentials))
            {
                Identity.ChangeLocalPassword(NewPassword);
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

        public string NewPassword
        {
            get => newPassword;
            set => SetProperty(ref newPassword, value);
        }
    }
}