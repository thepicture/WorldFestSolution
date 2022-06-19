using System.ComponentModel;
using System.Runtime.CompilerServices;
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

        /// <summary>
        /// Фоновая переменная 
        /// для хранения команды 
        /// изменения состояния 
        /// участия в фестивале.
        /// </summary>
        private Command toggleMyParticipateStateOfFestivalCommand;

        /// <summary>
        /// Свойство, оборачивающее 
        /// фоновую переменную 
        /// для хранения команды 
        /// изменения состояния 
        /// участия в фестивале.
        /// </summary>
        public ICommand ToggleMyParticipateStateOfFestivalCommand
        {
            // Геттер для возвращения
            // проинициализированной 
            // переменной.
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

        /// <summary>
        /// Переключает состояние участия в фестивале.
        /// </summary>
        private async void ToggleMyParticipateStateOfFestivalAsync()
        {
            // Оповещение представления о том, что 
            // происходит полезная работа.
            IsBusy = true;

            // Создание контейнера, описывающего 
            // участие в фестивале.
            UserOfFestival userOfFestival = new UserOfFestival
            {
                FestivalId = FestivalId,
                IsParticipating = Festival.IsMeParticipating
            };

            // Если участие успешно обработано,
            if (await UserOfFestivalDataStore.UpdateItemAsync(userOfFestival))
            {
                // то обновить страницу,
                IsRefreshing = true;
            }
            
            // и оповестить представление о том, 
            // что модель представления доступна.
            IsBusy = false;

            // В противном случае 
            // UserOfFestivalDataStore
            // покажет обратную связь пользователю.
        }

        internal void OnAppearing()
        {
            IsRefreshing = true;
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
            Festival festivalFromDatabase = await FestivalDataStore.GetItemAsync(
                FestivalId.ToString());
            if (festivalFromDatabase != null)
            {
                Festival = festivalFromDatabase;
                double rating = festivalFromDatabase.Rating;
                if (rating > 0)
                {
                    CurrentRating = $"Рейтинг: {rating:F2}";
                }
                else
                {
                    CurrentRating = "Фестиваль пока не оценивали";
                }
                Festival.Rating = rating;
                MessagingCenter.Instance.Send(this, "UpdateRatingBar", rating);
            }
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

        private Command<int> rateFestivalCommand;

        public Command<int> RateFestivalCommand
        {
            get
            {
                if (rateFestivalCommand == null)
                {
                    rateFestivalCommand = new Command<int>(RateFestivalAsync);
                }

                return rateFestivalCommand;
            }
        }

        private async void RateFestivalAsync(int countOfStars)
        {
            FestivalRating festivalRating = new FestivalRating
            {
                FestivalId = Festival.Id,
                CountOfStars = countOfStars,
                IsRated = Festival.IsRated
            };
            await FestivalRatingDataStore.AddItemAsync(festivalRating);
            IsRefreshing = true;
        }

        private Command goToParticipantsViewCommand;

        public ICommand GoToParticipantsViewCommand
        {
            get
            {
                if (goToParticipantsViewCommand == null)
                    goToParticipantsViewCommand = new Command(GoToParticipantsViewAsync);

                return goToParticipantsViewCommand;
            }
        }

        private async void GoToParticipantsViewAsync()
        {
            await Shell.Current.Navigation.PushAsync(
               new ParticipantsView(
                   new ParticipantsViewModel(FestivalId)));
        }

        private Command goToOrganizerAccountViewCommand;

        public ICommand GoToOrganizerAccountViewCommand
        {
            get
            {
                if (goToOrganizerAccountViewCommand == null)
                    goToOrganizerAccountViewCommand = new Command(GoToOrganizerAccountViewAsync);

                return goToOrganizerAccountViewCommand;
            }
        }

        private async void GoToOrganizerAccountViewAsync()
        {
            Festival festivalFromDataStore =
                await FestivalDataStore.GetItemAsync(
                    FestivalId.ToString());
            await Shell.Current.Navigation.PushAsync(
                new AccountView(
                    new AccountViewModel(festivalFromDataStore.OrganizerId)));
        }

        public string CurrentRating { get; set; }
    }
}