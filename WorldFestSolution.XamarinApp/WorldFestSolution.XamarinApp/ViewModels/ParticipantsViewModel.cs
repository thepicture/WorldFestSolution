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

        private Command<ParticipantUser> deleteParticipantCommand;

        public Command<ParticipantUser> DeleteParticipantCommand
        {
            get
            {
                if (deleteParticipantCommand == null)
                {
                    deleteParticipantCommand = new Command<ParticipantUser>(DeleteParticipantAsync);
                }

                return deleteParticipantCommand;
            }
        }

        private async void DeleteParticipantAsync(ParticipantUser user)
        {
            if (await FestivalUsersDataStore
                    .DeleteItemAsync(FestivalId + "," + user.Id))
            {
                IsRefreshing = true;
            }
        }

        private Command<ExtendedRatingBar> rateUserCommand;

        public Command<ExtendedRatingBar> RateUserCommand
        {
            get
            {
                if (rateUserCommand == null)
                    rateUserCommand = new Command<ExtendedRatingBar>(RateUserAsync);

                return rateUserCommand;
            }
        }

        private async void RateUserAsync(ExtendedRatingBar ratingBar)
        {
            UserRating rating = new UserRating
            {
                RaterId = Identity.Id,
                UserId = (ratingBar.BindingContext as ParticipantUser).Id,
                CountOfStars = (int)ratingBar.SelectedStarValue,
                IsRated = (ratingBar.BindingContext as ParticipantUser).IsRated
            };
            await UserRatingDataStore.AddItemAsync(rating);
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