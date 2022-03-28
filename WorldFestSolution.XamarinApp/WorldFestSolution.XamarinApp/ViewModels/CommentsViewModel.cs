using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using WorldFestSolution.XamarinApp.Models.Serialized;
using Xamarin.Forms;

namespace WorldFestSolution.XamarinApp.ViewModels
{
    public class CommentsViewModel : BaseViewModel
    {
        private ObservableCollection<Comment> comments;
        public CommentsViewModel(int festivalId)
        {
            Comments = new ObservableCollection<Comment>();
            FestivalId = festivalId;
        }

        public ObservableCollection<Comment> Comments
        {
            get => comments;
            set => SetProperty(ref comments, value);
        }

        internal void OnAppearing()
        {
            IsRefreshing = true;
        }

        private Command refreshCommand;
        private int festivalId;

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
            Festival festival = await FestivalDataStore
                .GetItemAsync(
                    FestivalId.ToString());
            if (festival != null)
            {
                foreach (Comment comment in festival.Comments)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Comments.Add(comment);
                    });
                    await Task.Delay(500);
                }
            }
            IsRefreshing = false;
        }
    }
}