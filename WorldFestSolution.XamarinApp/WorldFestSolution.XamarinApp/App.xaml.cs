using System.Net.Http;
using WorldFestSolution.XamarinApp.Models;
using WorldFestSolution.XamarinApp.Models.Serialized;
using WorldFestSolution.XamarinApp.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WorldFestSolution.XamarinApp
{
    public partial class App : Application
    {
        public static string Role { get; set; }
        public static User User { get; set; }
        public static string AuthorizationValue { get; set; }
        public static HttpClientHandler ClientHandler
        {
            get
            {
                HttpClientHandler handler = new HttpClientHandler();
                handler.ServerCertificateCustomValidationCallback +=
                    (_, __, ___, ____) => true;
                return handler;
            }
        }

        public App()
        {
            InitializeComponent();

            Accelerometer.Start(SensorSpeed.UI);
            Accelerometer.ShakeDetected += async (o, e) =>
            {
                string newBaseUrl = await App.Current.MainPage
                    .DisplayPromptAsync("Изменить путь к API",
                                        "Введите путь к API",
                                        initialValue: Api.BaseUrl);
                Models.Api.BaseUrl = newBaseUrl;
            };

            XF.Material.Forms.Material.Init(this,
                (XF
                .Material
                .Forms
                .Resources
                .MaterialConfiguration)Resources["CommonMaterial"]);

            DependencyService.Register<CredentialsService>();
            DependencyService.Register<AndroidAlertService>();
            DependencyService.Register<HttpClientAuthenticationService>();
            DependencyService.Register<HttpClientRegistrationService>();
            DependencyService.Register<FestivalDataStore>();
            DependencyService.Register<CommentDataStore>();
            DependencyService.Register<ChangePasswordDataStore>();
            DependencyService.Register<FestivalRatingDataStore>();
            DependencyService.Register<InviteDataStore>();
            DependencyService.Register<FestivalCommentDataStore>();
            DependencyService.Register<UserImageDataStore>();
            DependencyService.Register<FestivalResponseInviteDataStore>();
            DependencyService.Register<UserOfFestivalDataStore>();
            DependencyService.Register<FestivalPopularityDataStore>();
            DependencyService.Register<UserDataStore>();
            DependencyService.Register<UserFestivalDataStore>();
            DependencyService.Register<UserRatingDataStore>();

            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
