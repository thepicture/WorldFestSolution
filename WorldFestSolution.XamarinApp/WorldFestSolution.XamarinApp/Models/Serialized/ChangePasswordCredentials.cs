using WorldFestSolution.XamarinApp.Services;
using Xamarin.Forms;

namespace WorldFestSolution.XamarinApp.Models.Serialized
{

    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class ChangePasswordCredentials : BindableObject
    {
        public string Login { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }

        public bool IsOldPasswordHasError
        {
            get
            {
                if (string.IsNullOrWhiteSpace(OldPassword))
                {
                    OldPasswordErrorText = "Введите старый пароль";
                    return true;
                }
                else
                {
                    string oldAuthorizationValue = DependencyService
                        .Get<ILoginPasswordEncoder>()
                        .Encode(Login, OldPassword);
                    if (oldAuthorizationValue != Identity.AuthorizationValue)
                    {
                        OldPasswordErrorText = "Старый пароль не совпадает с текущим";
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }
        public string NewPasswordErrorText { get; set; }

        public bool IsNewPasswordHasError
        {
            get
            {
                if (string.IsNullOrWhiteSpace(NewPassword))
                {
                    NewPasswordErrorText = "Введите новый пароль";
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public string OldPasswordErrorText { get; set; }
    }
}
