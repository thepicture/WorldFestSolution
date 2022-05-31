using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using WorldFestSolution.XamarinApp.Controls;
using WorldFestSolution.XamarinApp.Models;
using WorldFestSolution.XamarinApp.Models.Serialized;
using WorldFestSolution.XamarinApp.Views;
using Xamarin.Forms;

namespace WorldFestSolution.XamarinApp.ViewModels
{
    public class ParticipantsViewModel : BaseViewModel
    {

        private Command refreshCommand;

        public ICommand RefreshCommand
        {
            get
            {
                if (refreshCommand == null)
                    refreshCommand = new Command(RefreshAsync);

                return refreshCommand;
            }
        }

        private async void RefreshAsync()
        {
            Participants.Clear();
            IEnumerable<User> dataStoreParticipants =
                await FestivalUsersDataStore.GetItemAsync(
                    FestivalId.ToString());
            foreach (ParticipantUser participant in dataStoreParticipants)
            {
                Participants.Add(participant);
            }
            IsRefreshing = false;
        }

        internal void OnAppearing()
        {
            IsRefreshing = true;
        }

        private ObservableCollection<ParticipantUser> competitors;

        public ParticipantsViewModel(int festivalId)
        {
            FestivalId = festivalId;
            Participants = new ObservableCollection<ParticipantUser>();
        }

        public ObservableCollection<ParticipantUser> Participants
        {
            get => competitors;
            set => SetProperty(ref competitors, value);
        }
        public int FestivalId { get; }

        /// <summary>
        /// Фоновая переменная 
        /// для хранения команды удаления 
        /// участника.
        /// </summary>
        private Command<ParticipantUser> deleteParticipantCommand;

        /// <summary>
        /// Оборачивает фоновую переменную 
        /// для команды удаления участника.
        /// </summary>
        public Command<ParticipantUser> DeleteParticipantCommand
        {
            // Попытка получить команду.
            get
            {
                /* Если команды нет, присвоить её фоновой 
                 * переменной и вернуть значение последней. */
                if (deleteParticipantCommand == null)
                {
                    deleteParticipantCommand =
                        new Command<ParticipantUser>(DeleteParticipantAsync);
                }

                // Иначе просто вернуть значение фоновой переменной.
                return deleteParticipantCommand;
            }
        }

        /// <summary>
        /// Удаляет участника из текущего фестиваля.
        /// </summary>
        /// <param name="user">Удаляемый участник.</param>
        private async void DeleteParticipantAsync(ParticipantUser user)
        {
            // Если участник удалён,
            if (await FestivalUsersDataStore
                    .DeleteItemAsync(FestivalId + "," + user.Id))
            {
                // То обновить страницу.
                IsRefreshing = true;
            }
            /* Иначе показать обратную связь, реализованную 
             * в FestivalsUsersDataStore. 
             * Это позволяет переиспользовать 
             * DataStore без дублирования логики. */
        }

        /// <summary>
        /// Фоновая переменная 
        /// для оценки пользователя.
        /// </summary>
        private Command<ExtendedRatingBar> rateUserCommand;

        /// <summary>
        /// Свойство, оборачивающее 
        /// фоновую переменную 
        /// для оценки пользователя.
        /// </summary>
        public Command<ExtendedRatingBar> RateUserCommand
        {
            // Геттер 
            // для инициализации
            // переменной.
            get
            {
                if (rateUserCommand == null)
                    rateUserCommand = new Command<ExtendedRatingBar>(RateUserAsync);

                return rateUserCommand;
            }
        }

        /// <summary>
        /// Оценивает пользователя по заданному 
        /// рейтинг-бару c контекстом.
        /// </summary>
        /// <param name="ratingBar">Рейтинг-бар с контекстом.</param>
        private async void RateUserAsync(ExtendedRatingBar ratingBar)
        {
            // Инициализация рейтинга 
            // по заданному рейтинг-бару.
            UserRating rating = new UserRating
            {
                RaterId = Identity.Id, // Кто оценивает.
                UserId = (ratingBar.BindingContext as ParticipantUser).Id, // Кого оценить.
                CountOfStars = (int)ratingBar.SelectedStarValue, // Число звёзд.
                IsRated = (ratingBar.BindingContext as ParticipantUser).IsRated // Был ли оценён ранее.
            };
            // Попытка отправить рейтинг 
            // на сервер.
            await UserRatingDataStore.AddItemAsync(rating);
            // Обновить страницу в любом случае
            // (так как новое значение может быть неактуально).
            IsRefreshing = true;
        }

        private Command<ParticipantUser> goToAccountViewCommand;

        public Command<ParticipantUser> GoToAccountViewCommand
        {
            get
            {
                if (goToAccountViewCommand == null)
                {
                    goToAccountViewCommand =
                        new Command<ParticipantUser>(GoToAccountViewAsync);
                }

                return goToAccountViewCommand;
            }
        }

        private async void GoToAccountViewAsync(ParticipantUser user)
        {
            await Shell.Current.Navigation.PushAsync(
                new AccountView(
                    new AccountViewModel(user.Id)));
        }
    }
}