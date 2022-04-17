using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using WorldFestSolution.XamarinApp.Models.Serialized;
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
                    inviteUserCommand = new Command<ResponseInvite>(InviteUserAsync);
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
    }
}