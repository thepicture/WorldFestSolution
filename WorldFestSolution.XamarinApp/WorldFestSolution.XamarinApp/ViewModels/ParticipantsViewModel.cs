﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using WorldFestSolution.XamarinApp.Models.Serialized;
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
            if(await FestivalUsersDataStore.DeleteItemAsync(FestivalId + "," + user.Id))
            {
                IsRefreshing = true;
            }
        }
    }
}