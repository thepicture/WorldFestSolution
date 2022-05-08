using Xamarin.Essentials;

namespace WorldFestSolution.XamarinApp.Models
{
    public class Api
    {
        private const string defaultBaseUrl =
            "https://worldfestsolution-webapi.conveyor.cloud/api/";

        public static string BaseUrl
        {
            get => Preferences.Get(
                nameof(BaseUrl), defaultBaseUrl);

            set => Preferences.Set(
                nameof(BaseUrl), value);
        }
    }
}
