using Newtonsoft.Json;
using WorldFestSolution.XamarinApp.Models.Serialized;
using WorldFestSolution.XamarinApp.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WorldFestSolution.XamarinApp.Models
{
    public class Identity
    {
        public static User User
        {
            get
            {
                if (App.User != null)
                {
                    return App.User;
                }
                else
                {
                    try
                    {
                        string user = SecureStorage.GetAsync("User").Result;
                        if (user == null)
                        {
                            return null;
                        }
                        return JsonConvert.DeserializeObject<User>(user);
                    }
                    catch (JsonReaderException ex)
                    {
                        DependencyService.Get<IAlertService>()
                            .InformError("Не удалось сохранить данные в системе. "
                            + "Ошибка: "
                            + ex.StackTrace);
                        return null;
                    }
                }
            }
            set
            {
                value.Image = null;
                App.User = value;
                try
                {

                    string serializedUser = JsonConvert.SerializeObject(value);
                    SecureStorage
                        .SetAsync("User", serializedUser)
                        .Wait();
                }
                catch (JsonReaderException ex)
                {
                    DependencyService.Get<IAlertService>()
                        .InformError("Не удалось "
                        + "сохранить пользователя в системе. "
                        + "Ошибка: "
                        + ex.StackTrace);
                }
            }
        }

        internal static void ChangeLocalPassword(string newPassword)
        {
            string newAuthorizationValue = DependencyService
                .Get<ICredentialsService>()
                .Encode(User.Login, newPassword);
            if (App.Identity != null)
            {
                App.Identity = newAuthorizationValue;
            }
            else
            {
                _ = SecureStorage.SetAsync("Identity", newAuthorizationValue);
            }
        }

        internal static void Logout()
        {
            App.User = null;
            App.Identity = null;
            SecureStorage.RemoveAll();
        }

        public static string Role
        {
            get
            {
                return User.UserTypeId == 1
                    ? "Участник"
                    : "Организатор";
            }
        }

        public static bool IsOrganizer => Role == "Организатор";
        public static bool IsParticipant => Role == "Участник";
        public static int Id => User.Id;
        public static string AuthorizationValue
        {
            get
            {
                if (App.Identity != null)
                {
                    return App.Identity;
                }
                else
                {
                    return SecureStorage.GetAsync("Identity").Result;
                }
            }
            set
            {
                App.Identity = value;
                if (value == null)
                {
                    _ = SecureStorage.Remove("Identity");
                }
                else
                {
                    _ = SecureStorage.SetAsync("Identity", value);
                }
            }
        }
    }
}
