using WorldFestSolution.XamarinApp.Models;
using WorldFestSolution.XamarinApp.ViewModels;
using WorldFestSolution.XamarinApp.Views;
using Xamarin.Forms;

namespace WorldFestSolution.XamarinApp
{
    public partial class AppShell : Shell
    {
        private static TabBar TabBar;
        public AppShell()
        {
            InitializeComponent();

            TabBar = new TabBar();
            Items.Add(TabBar);

            if (IsLoggedIn())
            {
                SetShellStacksDependingOnRole();
            }
            else
            {
                LoadLoginAndRegisterShell();
            }
        }

        public static void LoadLoginAndRegisterShell()
        {
            TabBar.Items.Clear();
            TabBar
                .Items.Add(new ShellContent
                {
                    Route = nameof(LoginView),
                    Icon = "login_bar",
                    Title = "Авторизация",
                    ContentTemplate = new DataTemplate(
                        typeof(LoginView))
                });
            TabBar
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
            return Identity.User != null;
        }

        public static void SetShellStacksDependingOnRole()
        {
            TabBar.Items.Clear();
            TabBar.Items.Add(new ShellContent
            {
                Route = $"My{nameof(FestivalsView)}",
                Icon = "programs",
                Title = "Мои фестивали",
                ContentTemplate = new DataTemplate(() =>
                {
                    return new FestivalsView(
                        new FestivalsViewModel(isRelatedToMe: true));
                })
            });
            switch (Identity.Role)
            {
                case "Организатор":
                    TabBar.Items.Add(new ShellContent
                    {
                        Route = nameof(FestivalsPopularityChartView),
                        Icon = "icon_feed",
                        Title = "Популярность фестов",
                        ContentTemplate = new DataTemplate(
                            typeof(FestivalsPopularityChartView)),
                    });
                    break;
                case "Участник":
                    TabBar.Items.Add(new ShellContent
                    {
                        Route = nameof(FestivalsView),
                        Icon = "logo",
                        Title = "Поиск фестивалей",
                        ContentTemplate = new DataTemplate(() =>
                        {
                            return new FestivalsView(
                                new FestivalsViewModel(isRelatedToMe: false));
                        })
                    });
                    break;
                default:
                    break;
            }
            TabBar.Items.Add(new ShellContent
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