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
                if (SecureStorage.GetAsync("User").Result is string value)
                {
                    return JsonConvert.DeserializeObject<User>(value);
                }
                else
                {
                    return App.User;
                }
            }
            set
            {
                value.Image = null;
                App.User = value;

                string serializedUser = JsonConvert.SerializeObject(value);
                SecureStorage
                    .SetAsync("User", serializedUser);
            }
        }

        internal static void Logout()
        {
            App.User = null;
            App.AuthorizationValue = null;
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
                if (SecureStorage.GetAsync("AuthorizationValue").Result
                    is string value)
                {
                    return value;
                }
                else
                {
                    return App.AuthorizationValue;
                }
            }
            set
            {
                App.AuthorizationValue = value;
                _ = SecureStorage.SetAsync("AuthorizationValue", value);
            }
        }

        public static bool IsLoggedIn => AuthorizationValue != null;
    }
}
