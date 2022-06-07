using Plugin.LocalNotification;
using Plugin.LocalNotification.AndroidOption;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using WorldFestSolution.XamarinApp.Models;
using WorldFestSolution.XamarinApp.Models.Serialized;
using WorldFestSolution.XamarinApp.Services;
using Xamarin.Essentials;
using Xamarin.Forms;
using XF.Material.Forms;
using XF.Material.Forms.Resources;

namespace WorldFestSolution.XamarinApp
{
    public partial class App : Application
    {
        public static int ImageWidth = 500;
        public static int ImageHeight = 500;
        public static int ImageQuality = 50;

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

        public int LastCountOfMyInvites
        {
            get => Preferences.Get(nameof(LastCountOfMyInvites), 0);
            set => Preferences.Set(nameof(LastCountOfMyInvites), value);
        }

        public static readonly string Json = "application/json";

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

            Material.Init(this, (MaterialConfiguration)Resources["CommonMaterial"]);

            DependencyService.Register<CredentialsService>();
            DependencyService.Register<AndroidAlertService>();
            DependencyService.Register<LoginUserDataStore>();
            DependencyService.Register<RegistrationUserDataStore>();
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
            DependencyService.Register<FestivalUsersDataStore>();
            DependencyService.Register<ImageTransformService>();
            DependencyService.Register<CountOfMyInvitesDataStore>();

            MainPage = new AppShell();
            Device.StartTimer(
                TimeSpan.FromSeconds(5),
                ShowNotificationsAndContinueIfHasMore);
        }

        private bool ShowNotificationsAndContinueIfHasMore()
        {
            Task.Run(async () =>
            {
                if (Identity.IsLoggedIn && Identity.IsParticipant)
                {
                    int countOfMyInvites = await DependencyService
                        .Get<IDataStore<int>>()
                        .GetItemAsync("");
                    if (countOfMyInvites > LastCountOfMyInvites)
                    {
                        Debug.WriteLine("Notify");
                        var notification = new NotificationRequest
                        {
                            NotificationId = 100,
                            Title = "У вас новые приглашения",
                            Description = "Зайдите в свои приглашения для просмотра",
                            Android =
                            {
                                Priority = AndroidNotificationPriority.High,
                                VibrationPattern = new long[] {0, 500 }
                            }
                        };
                        await NotificationCenter.Current.Show(notification);
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            LastCountOfMyInvites = countOfMyInvites;
                        });
                    }
                }
            });
            return true;
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
