using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using WorldFestSolution.XamarinApp.Models.Serialized;
using WorldFestSolution.XamarinApp.Services;
using Xamarin.Forms;

namespace WorldFestSolution.XamarinApp.ViewModels
{
    public class UserChatViewModel : BaseViewModel
    {
        internal void OnAppearing()
        {
            IsRefreshing = true;
        }

        public IDataStore<UserMessage> UserMessageDataStore =>
          DependencyService.Get<IDataStore<UserMessage>>();
        public IDataStore<IEnumerable<UserMessage>> UserMessagesDataStore =>
            DependencyService.Get<IDataStore<IEnumerable<UserMessage>>>();

        public UserChatViewModel(int userId)
        {
            ReceiverId = userId;
            Message = new UserMessage
            {
                ReceiverId = ReceiverId
            };
        }

        public int ReceiverId { get; }

        private Command postMessageCommand;

        public ICommand PostMessageCommand
        {
            get
            {
                if (postMessageCommand == null)
                    postMessageCommand = new Command(PostMessageAsync);

                return postMessageCommand;
            }
        }

        private async void PostMessageAsync()
        {
            if (await UserMessageDataStore.AddItemAsync(Message))
            {
                Message.Message = string.Empty;
                IsRefreshing = true;
            }
        }

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

        public ObservableCollection<UserMessage> Messages { get; set; }
        public UserMessage Message { get; set; }

        private async void RefreshAsync()
        {
            await LoadMessagesAsync();
        }

        private async Task LoadMessagesAsync()
        {
            string receiverIdAsString = ReceiverId.ToString();
            IEnumerable<UserMessage> currentMessages = await UserMessagesDataStore.GetItemAsync(receiverIdAsString);

            Messages = new ObservableCollection<UserMessage>(currentMessages);

            IsRefreshing = false;
        }
    }
}