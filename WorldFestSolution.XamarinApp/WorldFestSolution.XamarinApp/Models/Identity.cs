using Newtonsoft.Json;
using WorldFestSolution.XamarinApp.Models.Serialized;
using Xamarin.Essentials;

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
                    return JsonConvert.DeserializeObject<User>(
                        SecureStorage.GetAsync("User").Result);
                }
            }
            set
            {
                (App.Current as App).User = value;
                _ = SecureStorage.SetAsync(
                    "User", JsonConvert.SerializeObject(value));
            }
        }

        internal static void Logout()
        {
            (App.Current as App).User = null;
            SecureStorage.RemoveAll();
        }

        public static string Role => User.UserTypeId == 1
            ? "Участник"
            : "Организатор";
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
