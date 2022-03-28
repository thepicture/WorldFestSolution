﻿using WorldFestSolution.XamarinApp.Models.Serialized;
using WorldFestSolution.XamarinApp.Services;
using Xamarin.Forms;

namespace WorldFestSolution.XamarinApp
{
    public partial class App : Application
    {
        public string Role { get; set; }
        public string Identity { get; set; }
        public User User { get; set; }

        public App()
        {
            InitializeComponent();
            XF.Material.Forms.Material.Init(this,
                (XF
                .Material
                .Forms
                .Resources
                .MaterialConfiguration)Resources["CommonMaterial"]);

            DependencyService.Register<AndroidAlertService>();
            DependencyService.Register<HttpClientAuthenticationService>();
            DependencyService.Register<HttpClientRegistrationService>();
            DependencyService.Register<FestivalDataStore>();
            DependencyService.Register<CommentDataStore>();
            DependencyService.Register<InviteOfFestivalDataStore>();

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
