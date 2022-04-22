﻿using WorldFestSolution.XamarinApp.Models.Serialized;
using WorldFestSolution.XamarinApp.Services;
using Xamarin.Forms;

namespace WorldFestSolution.XamarinApp
{
    public partial class App : Application
    {
        public static string Role { get; set; }
        public static User User { get; set; }
        public static string AuthorizationValue { get; set; }

        public App()
        {
            InitializeComponent();
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
            DependencyService.Register<InviteOfFestivalDataStore>();
            DependencyService.Register<ChangePasswordDataStore>();
            DependencyService.Register<FestivalRatingDataStore>();
            DependencyService.Register<InviteDataStore>();
            DependencyService.Register<FestivalCommentDataStore>();
            DependencyService.Register<UserImageDataStore>();

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
