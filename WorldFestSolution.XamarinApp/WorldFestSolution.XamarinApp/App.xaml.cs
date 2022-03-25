using WorldFestSolution.XamarinApp.Services;
using Xamarin.Forms;

namespace WorldFestSolution.XamarinApp
{
    public partial class App : Application
    {
        public string Role { get; set; }
        public string Identity { get; set; }

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
