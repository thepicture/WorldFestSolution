using System.IO;
using System.Windows.Input;
using WorldFestSolution.XamarinApp.Models;
using WorldFestSolution.XamarinApp.Models.Serialized;
using WorldFestSolution.XamarinApp.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WorldFestSolution.XamarinApp.ViewModels
{
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class AccountViewModel : BaseViewModel
    {

        private Command logoutCommand;

        public ICommand LogoutCommand
        {
            get
            {
                if (logoutCommand == null)
                {
                    logoutCommand = new Command(LogoutAsync);
                }

                return logoutCommand;
            }
        }

        internal void OnAppearing()
        {
            IsRefreshing = true;
        }

        private async void LogoutAsync()
        {
            if (await AlertService.Ask("Выйти из аккаунта?"))
            {
                Identity.Logout();
                AppShell.LoadLoginAndRegisterShell();
            }
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
            User userFromDatabase = await UserDataStore.GetItemAsync(
                UserId.ToString());
            double countOfStars = userFromDatabase.Rating;
            if (userFromDatabase != null)
            {
                CurrentUser = userFromDatabase;
                CurrentUser.Rating = countOfStars;
            }
            IsRefreshing = false;
        }

        private Command goToChangePasswordViewCommand;

        public ICommand GoToChangePasswordViewCommand
        {
            get
            {
                if (goToChangePasswordViewCommand == null)
                {
                    goToChangePasswordViewCommand =
                        new Command(GoToChangePasswordView);
                }

                return goToChangePasswordViewCommand;
            }
        }

        private async void GoToChangePasswordView()
        {
            await Shell.Current.Navigation.PushAsync(
                new ChangePasswordView());
        }

        private Command goToMyInvitesViewCommand;

        public ICommand GoToMyInvitesViewCommand
        {
            get
            {
                if (goToMyInvitesViewCommand == null)
                {
                    goToMyInvitesViewCommand =
                        new Command(GoToMyInvitesViewAsync);
                }

                return goToMyInvitesViewCommand;
            }
        }

        private async void GoToMyInvitesViewAsync()
        {
            await Shell.Current.Navigation.PushAsync(
               new InviteView(
                   new InviteViewModel(festivalId: 0)));
        }

        private Command changeImageCommand;
        private User currentUser;

        public AccountViewModel()
        {
            UserId = User.Id;
        }
        public AccountViewModel(int userId)
        {
            UserId = userId;
        }

        public ICommand ChangeImageCommand
        {
            get
            {
                if (changeImageCommand == null)
                {
                    changeImageCommand = new Command(ChangeImageAsync);
                }

                return changeImageCommand;
            }
        }

        public int UserId { get; }
        public User CurrentUser
        {
            get => currentUser;
            set => SetProperty(ref currentUser, value);
        }

        private async void ChangeImageAsync()
        {
            FileResult imageResult = await MediaPicker
                .PickPhotoAsync(new MediaPickerOptions
                {
                    Title = "Выберите новое фото"
                });
            if (imageResult == null)
            {
                await AlertService.Warn("Изменение фото отменено");
                return;
            }

            Stream imageStream = await imageResult.OpenReadAsync();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                await imageStream.CopyToAsync(memoryStream);
                if (await UserImageDataStore.AddItemAsync(
                        ImageTransformService.Transform(
                            memoryStream.ToArray(),
                            App.ImageWidth,
                            App.ImageHeight,
                            App.ImageQuality)))
                {
                    IsRefreshing = true;
                }
            }
        }

        private Command<int> rateUserCommand;

        public Command<int> RateUserCommand
        {
            get
            {
                if (rateUserCommand == null)
                    rateUserCommand = new Command<int>(RateUserAsync);

                return rateUserCommand;
            }
        }

        private async void RateUserAsync(int countOfStars)
        {
            UserRating rating = new UserRating
            {
                RaterId = CurrentUser.Id,
                UserId = CurrentUser.Id,
                CountOfStars = countOfStars,
                IsRated = CurrentUser.IsRated
            };
            await UserRatingDataStore.AddItemAsync(rating);
            IsRefreshing = true;
        }
    }
}