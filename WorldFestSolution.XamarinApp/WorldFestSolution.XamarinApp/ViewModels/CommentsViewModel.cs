using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            Comment = new Comment
            {
                FestivalId = festivalId
            };
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
        public Comment Comment
        {
            get => comment;
            set => SetProperty(ref comment, value);
        }

        private async void RefreshAsync()
        {
            Comments.Clear();
            IEnumerable<Comment> currentComments =
                await FestivalCommentDataStore
                .GetItemAsync(
                    FestivalId.ToString());
            {
                foreach (Comment currentComment in currentComments)
                {
                    Comments.Add(currentComment);
                }
            }
            IsRefreshing = false;
        }

        private Command postCommentCommand;

        public ICommand PostCommentCommand
        {
            get
            {
                if (postCommentCommand == null)
                {
                    postCommentCommand = new Command(PostCommentAsync);
                }

                return postCommentCommand;
            }
        }

        private async void PostCommentAsync()
        {
            if (await CommentDataStore.AddItemAsync(Comment))
            {
                Comment.Text = string.Empty;
                OnPropertyChanged(nameof(Comment));
                IsRefreshing = true;
            }
        }

        private Command<Comment> deleteCommentCommand;
        private Comment comment;

        public Command<Comment> DeleteCommentCommand
        {
            get
            {
                if (deleteCommentCommand == null)
                {
                    deleteCommentCommand
                        = new Command<Comment>(DeleteCommentAsync);
                }

                return deleteCommentCommand;
            }
        }

        private async void DeleteCommentAsync(Comment comment)
        {
            if (await CommentDataStore.DeleteItemAsync(
                comment.Id.ToString()))
            {
                IsRefreshing = true;
            }
        }
    }
}