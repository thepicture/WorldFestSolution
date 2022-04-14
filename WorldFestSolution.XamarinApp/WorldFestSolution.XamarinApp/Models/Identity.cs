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
                if ((App.Current as App).User != null)
                {
                    return (App.Current as App).User;
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
                (App.Current as App).User = value;
                _ = SecureStorage.SetAsync(
                    "User", JsonConvert.SerializeObject(value));
            }
        }

        internal static void ChangeLocalPassword(string newPassword)
        {
            string newAuthorizationValue = CredentialsToBasicConverter
                                .Encode(User.Login, newPassword);
            if ((App.Current as App).Identity != null)
            {
                (App.Current as App).Identity = newAuthorizationValue;
            }
            else
            {
                _ = SecureStorage.SetAsync("Identity", newAuthorizationValue);
            }
        }

        internal static void Logout()
        {
            (App.Current as App).User = null;
            (App.Current as App).Identity = null;
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
                if ((App.Current as App).Identity != null)
                {
                    return (App.Current as App).Identity;
                }
                else
                {
                    return SecureStorage.GetAsync("Identity").Result;
                }
            }
            set
            {
                (App.Current as App).Identity = value;
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
