using System;
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
                    ContentTemplate = new DataTemplate(typeof(LoginView))
                });
            ShellContentTabBar
              .Items.Add(new ShellContent
              {
                  Route = nameof(RegisterView),
                  Icon = "register_bar",
                  Title = "Регистрация",
                  ContentTemplate = new DataTemplate(typeof(RegisterView))
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
            switch (Identity.Role)
            {
                case "Организатор":
                    break;
                case "Участник":
                    break;
                default:
                    break;
            }
        }
    }
}