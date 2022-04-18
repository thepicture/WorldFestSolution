using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using WorldFestSolution.XamarinApp.Models.Serialized;
using WorldFestSolution.XamarinApp.Views;
using Xamarin.Forms;

namespace WorldFestSolution.XamarinApp.ViewModels
{
    public class InviteViewModel : BaseViewModel
    {
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

        private async void RefreshAsync()
        {
            if (Invites.Count > 0)
            {
                Invites.Clear();
            }

            IEnumerable<ResponseInvite> dataStoreInvites =
                await InviteDataStore.GetItemsAsync();
            if (dataStoreInvites != null)
            {
                foreach (ResponseInvite invite in dataStoreInvites)
                {
                    Invites.Add(invite);
                }
            }
            IsRefreshing = false;
        }

        private ObservableCollection<ResponseInvite> invites;

        public ObservableCollection<ResponseInvite> Invites
        {
            get => invites;
            set => SetProperty(ref invites, value);
        }

        private Command<ResponseInvite> inviteUserCommand;

        public InviteViewModel(int festivalId)
        {
            Invites = new ObservableCollection<ResponseInvite>();
            FestivalId = festivalId;
        }

        public Command<ResponseInvite> InviteUserCommand
        {
            get
            {
                if (inviteUserCommand == null)
                {
                    inviteUserCommand =
                        new Command<ResponseInvite>(InviteUserAsync);
                }

                return inviteUserCommand;
            }
        }

        public int FestivalId { get; }

        private async void InviteUserAsync(ResponseInvite invite)
        {
            if (await InviteDataStore.AddItemAsync(invite))
            {
                IsRefreshing = true;
            }
        }

        private Command<ResponseInvite> acceptInviteCommand;

        public Command<ResponseInvite> AcceptInviteCommand
        {
            get
            {
                if (acceptInviteCommand == null)
                {
                    acceptInviteCommand =
                        new Command<ResponseInvite>(AcceptInviteAsync);
                }

                return acceptInviteCommand;
            }
        }

        private async void AcceptInviteAsync(ResponseInvite invite)
        {
            invite.IsAccepted = true;
            if (await InviteDataStore.AddItemAsync(invite))
            {
                NavigateToFestivalAsync(invite.FestivalId);
            }
            else
            {
                invite.IsAccepted = false;
            }
        }

        private async void NavigateToFestivalAsync(int festivalId)
        {
            await Shell.Current.Navigation.PushAsync(
                new FestivalView(
                    new FestivalViewModel(festivalId)));
        }

        private Command<ResponseInvite> rejectInviteCommand;

        public Command<ResponseInvite> RejectInviteCommand
        {
            get
            {
                if (rejectInviteCommand == null)
                {
                    rejectInviteCommand =
                        new Command<ResponseInvite>(RejectInviteAsync);
                }

                return rejectInviteCommand;
            }
        }

        private async void RejectInviteAsync(ResponseInvite invite)
        {
            if (await InviteDataStore.AddItemAsync(invite))
            {
                IsRefreshing = true;
            }
        }
    }
}