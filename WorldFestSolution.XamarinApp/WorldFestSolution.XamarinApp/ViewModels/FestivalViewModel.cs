using System.Windows.Input;
using WorldFestSolution.XamarinApp.Models.Serialized;
using WorldFestSolution.XamarinApp.Views;
using Xamarin.Forms;

namespace WorldFestSolution.XamarinApp.ViewModels
{
    public class FestivalViewModel : BaseViewModel
    {
        private int festivalId;
        private Festival festival;

        public FestivalViewModel(int festivalId)
        {
            FestivalId = festivalId;
        }

        public Festival Festival
        {
            get => festival;
            set => SetProperty(ref festival, value);
        }

        private Command toggleMyParticipateStateOfFestivalCommand;

        public ICommand ToggleMyParticipateStateOfFestivalCommand
        {
            get
            {
                if (toggleMyParticipateStateOfFestivalCommand == null)
                {
                    toggleMyParticipateStateOfFestivalCommand = new Command(
                        ToggleMyParticipateStateOfFestivalAsync);
                }

                return toggleMyParticipateStateOfFestivalCommand;
            }
        }

        internal void OnAppearing()
        {
            IsRefreshing = true;
        }

        private async void ToggleMyParticipateStateOfFestivalAsync()
        {
            IsBusy = true;
            if (await InviteOfFestivalDataStore
                .ToggleParticipateAsync(FestivalId))
            {
                string action = Festival.IsMeParticipating
                    ? "покинули"
                    : "вступили в";
                await AlertService
                    .InformError($"Вы {action} фестиваль");
                IsRefreshing = true;
            }
            else
            {
                string action = Festival.IsMeParticipating
                    ? "покинуть"
                    : "вступить в";
                await AlertService
                    .InformError($"Не удалось {action} фестиваль");
            }
            IsBusy = false;
        }

        private Command refreshCommand;

        public ICommand RefreshCommand
        {
            get
            {
                if (refreshCommand == null)
                {
                    refreshCommand = new Command(RefreshAsync);
                }

                return refreshCommand;
            }
        }

        public int FestivalId
        {
            get => festivalId;
            set => SetProperty(ref festivalId, value);
        }

        private async void RefreshAsync()
        {
            Festival = await FestivalDataStore.GetItemAsync(
                FestivalId.ToString());
            IsRefreshing = false;
        }

        private Command goToCommentsViewCommand;

        public ICommand GoToCommentsViewCommand
        {
            get
            {
                if (goToCommentsViewCommand == null)
                {
                    goToCommentsViewCommand = new Command(GoToCommentsViewAsync);
                }

                return goToCommentsViewCommand;
            }
        }

        private async void GoToCommentsViewAsync()
        {
            await Shell.Current.Navigation.PushAsync(
                new CommentsView(
                    new CommentsViewModel(FestivalId)));
        }

        private Command goToEditFestivalViewCommand;

        public ICommand GoToEditFestivalViewCommand
        {
            get
            {
                if (goToEditFestivalViewCommand == null)
                {
                    goToEditFestivalViewCommand =
                        new Command(GoToEditFestivalViewAsync);
                }

                return goToEditFestivalViewCommand;
            }
        }

        private async void GoToEditFestivalViewAsync()
        {
            if (Festival == null)
            {
                await AlertService.Warn("Фестиваль ещё не загрузился");
                return;
            }
            await Shell.Current.Navigation.PushAsync(
                new AddEditFestivalView(
                    new AddEditFestivalViewModel(Festival)));
        }

        private Command goToProgramsViewCommand;

        public ICommand GoToProgramsViewCommand
        {
            get
            {
                if (goToProgramsViewCommand == null)
                {
                    goToProgramsViewCommand =
                        new Command(GoToProgramsViewAsync);
                }

                return goToProgramsViewCommand;
            }
        }

        private async void GoToProgramsViewAsync()
        {
            await Shell.Current.Navigation.PushAsync(
                new ProgramsView(
                    new ProgramsViewModel(Festival)));
        }

        private Command goToInviteViewCommand;

        public ICommand GoToInviteViewCommand
        {
            get
            {
                if (goToInviteViewCommand == null)
                {
                    goToInviteViewCommand = new Command(GoToInviteViewAsync);
                }

                return goToInviteViewCommand;
            }
        }

        private async void GoToInviteViewAsync()
        {
            await Shell.Current.Navigation.PushAsync(
                new InviteView(
                    new InviteViewModel(FestivalId)));
        }
    }
}