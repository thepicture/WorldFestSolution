using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

            IEnumerable<Invite> dataStoreInvites =
                await InviteDataStore.GetItemsAsync();
            if (dataStoreInvites != null)
            {
                foreach (Invite invite in dataStoreInvites)
                {
                    Invites.Add(invite);
                }
            }
            IsRefreshing = false;
        }

        private ObservableCollection<Invite> invites;

        public ObservableCollection<Invite> Invites
        {
            get => invites;
            set => SetProperty(ref invites, value);
        }

        private Command inviteUserCommand;

        public InviteViewModel(int festivalId)
        {
            Invites = new ObservableCollection<Invite>();
            FestivalId = festivalId;
        }

        public ICommand InviteUserCommand
        {
            get
            {
                if (inviteUserCommand == null)
                {
                    inviteUserCommand = new Command(InviteUserAsync);
                }

                return inviteUserCommand;
            }
        }

        public int FestivalId { get; }

        private void InviteUserAsync()
        {
        }
    }
}