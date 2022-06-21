using System.Windows.Input;
using Xamarin.Forms;

namespace WorldFestSolution.XamarinApp.Models.Specialized
{
    public class ItemTutorial
    {
        public string Text { get; set; }
        public string ImagePath { get; set; }
        public bool IsLast { get; set; }

        private Command goToLoginViewCommand;

        public ICommand GoToLoginViewCommand
        {
            get
            {
                if (goToLoginViewCommand == null)
                    goToLoginViewCommand = new Command(GoToLoginView);

                return goToLoginViewCommand;
            }
        }

        private void GoToLoginView()
        {
            AppShell.LoadLoginAndRegisterShell();
        }
    }
}
