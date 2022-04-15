using WorldFestSolution.XamarinApp.Models;
using WorldFestSolution.XamarinApp.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WorldFestSolution.XamarinApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            if (IsLoggedIn())
            {
                SetShellStacksDependingOnRole();
            }
            else
            {
                LoadLoginAndRegisterShell();
            }
        }

        public void LoadLoginAndRegisterShell()
        {
            ShellContentTabBar.Items.Clear();
            ShellContentTabBar
                .Items.Add(new ShellContent
                {
                    Route = nameof(LoginView),
                    Icon = "login_bar",
                    Title = "Авторизация",
                    ContentTemplate = new DataTemplate(
                        typeof(LoginView))
                });
            ShellContentTabBar
              .Items.Add(new ShellContent
              {
                  Route = nameof(RegisterView),
                  Icon = "register_bar",
                  Title = "Регистрация",
                  ContentTemplate = new DataTemplate(
                      typeof(RegisterView))
              });
        }

        private bool IsLoggedIn()
        {
            return SecureStorage
                .GetAsync("Identity").Result != null;
        }

        internal void SetShellStacksDependingOnRole()
        {
            ShellContentTabBar.Items.Clear();
            ShellContentTabBar.Items.Add(new ShellContent
            {
                Route = $"My{nameof(FestivalsView)}",
                Icon = "programs",
                Title = "Мои фестивали",
                ContentTemplate = new DataTemplate(
                    typeof(FestivalsView)),
            });
            switch (Identity.Role)
            {
                case "Организатор":
                    ShellContentTabBar.Items.Add(new ShellContent
                    {
                        Route = nameof(FestivalsPopularityChartView),
                        Icon = "icon_feed",
                        Title = "Популярность фестов",
                        ContentTemplate = new DataTemplate(
                            typeof(FestivalsPopularityChartView)),
                    });
                    break;
                case "Участник":
                    ShellContentTabBar.Items.Add(new ShellContent
                    {
                        Route = nameof(FestivalsView),
                        Icon = "logo",
                        Title = "Поиск фестивалей",
                        ContentTemplate = new DataTemplate(
                            typeof(FestivalsView)),
                    });
                    break;
                default:
                    break;
            }
            ShellContentTabBar.Items.Add(new ShellContent
            {
                Route = nameof(AccountView),
                Icon = "login",
                Title = "Аккаунт",
                ContentTemplate = new DataTemplate(
                    typeof(AccountView))
            });
        }
    }
}