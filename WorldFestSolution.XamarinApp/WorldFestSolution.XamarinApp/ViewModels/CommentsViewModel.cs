﻿using System.Collections.ObjectModel;
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
                Device.BeginInvokeOnMainThread(() =>
                {
                    Comments.Clear();
                });
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

        private string commentText;

        public string CommentText
        {
            get => commentText;
            set => SetProperty(ref commentText, value);
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
            if (string.IsNullOrWhiteSpace(CommentText))
            {
                await AlertService.InformError("Введите комментарий");
                return;
            }
            Comment newComment = new Comment
            {
                Text = CommentText,
                FestivalId = FestivalId
            };
            if (await CommentDataStore.AddItemAsync(newComment))
            {
                CommentText = string.Empty;
                IsRefreshing = true;
            }
        }
    }
}