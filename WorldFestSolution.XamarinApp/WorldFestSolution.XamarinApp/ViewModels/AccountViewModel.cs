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
        public bool IsCurrentUserNotMe { get; set; }

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
            if (IsBusy)
            {
                await AlertService.Warn("Вы не можете "
                                        + "выйти из аккаунта "
                                        + "до окончания обновления страницы");
                return;
            }
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

        public bool IsWantsInvites
        {
            get => isWantsInvites;
            set => SetProperty(ref isWantsInvites, value);
        }

        private async void RefreshAsync()
        {
            IsBusy = true;
            User userFromDatabase = await UserDataStore.GetItemAsync(
                UserId.ToString());
            if (userFromDatabase != null)
            {
                double countOfStars = userFromDatabase.Rating;
                if (countOfStars > 0)
                {
                    CurrentRating = $"Рейтинг: {countOfStars:F2}";
                }
                else
                {
                    CurrentRating = "Пользователя пока не оценивали";
                }
                CurrentUser = userFromDatabase;
                IsWantsInvites = userFromDatabase.IsWantsInvites;
                CurrentUser.Rating = countOfStars;
                IsSelf = CurrentUser.Id == Identity.Id;
                MessagingCenter.Instance.Send(this, "UpdateRatingBar", countOfStars);
                IsCurrentUserNotMe = Identity.Id != userFromDatabase.Id;
            }
            IsRefreshing = false;
            IsBusy = false;
        }

        public string CurrentRating
        {
            get => currentRating;
            set => SetProperty(ref currentRating, value);
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

        public AccountViewModel() : this(Identity.Id) { }
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

        private bool isSelf = true;
        private bool isWantsInvites;

        public bool IsSelf { get => isSelf; set => SetProperty(ref isSelf, value); }

        /// <summary>
        /// Фоновая переменная для изменения 
        /// состояния принятия приглашений 
        /// от организаторов.
        /// </summary>
        private Command<bool> inviteStateChangedCommand;

        /// <summary>
        /// Свойство, оборачивающее 
        /// фоновую переменную для изменения 
        /// состояния принятия приглашений 
        /// от организаторов.
        /// </summary>
        public Command<bool> InviteStateChangedCommand
        {
            // Геттер для инициализации 
            // фоновой переменной.
            get
            {
                if (inviteStateChangedCommand == null)
                    inviteStateChangedCommand =
                        new Command<bool>(OnInviteStateChanged);

                return inviteStateChangedCommand;
            }
        }

        /// <summary>
        /// Изменяет состояние принятия 
        /// приглашений от организаторов.
        /// </summary>
        /// <param name="newValue">Логическое значение, 
        /// описывающее желание принятия 
        /// приглашений от организаторов.</param>
        private async void OnInviteStateChanged(bool newValue)
        {
            /*
             * Если текущий пользователь существует 
             * (для предотвращения несуществующей ссылки, если 
             * данные о пользователе не загрузились) и
             * если текущий пользователь является 
             * авторизованным пользователем, 
             * при этом значение отличается от предыдущего 
             * (чтобы не обновлять страницу без причины),
             */
            if (CurrentUser != null
                && CurrentUser.Id == User.Id
                && CurrentUser.IsWantsInvites != newValue)
            {
                // то присвоить свойству новое значение переменной.
                CurrentUser.IsWantsInvites = newValue;

                // Попытка сохранить новое состояние.
                if (await UserDataStore.UpdateItemAsync(CurrentUser))
                {
                    // Если сохранено, то обновить страницу.
                    IsRefreshing = true;
                }
                // В противном случае 
                // UserDataStore
                // покажет обратную связь
                // пользователю.
            }
        }

        private Command goToUsersPopularityViewCommand;

        public ICommand GoToUsersPopularityViewCommand
        {
            get
            {
                if (goToUsersPopularityViewCommand == null)
                    goToUsersPopularityViewCommand = new Command(GoToUsersPopularityViewAsync);

                return goToUsersPopularityViewCommand;
            }
        }

        private async void GoToUsersPopularityViewAsync()
        {
            await Shell.Current.Navigation.PushAsync(
               new UsersPopularityView());
        }

        private Command goToUserChatView;
        private string currentRating;

        public ICommand GoToUserChatView
        {
            get
            {
                if (goToUserChatView == null)
                    goToUserChatView = new Command(PerformGoToUserChatViewAsync);

                return goToUserChatView;
            }
        }

        private async void PerformGoToUserChatViewAsync()
        {
            await Shell.Current.Navigation.PushAsync(
               new UserChatView(
                   new UserChatViewModel(userId: CurrentUser.Id)));
        }
    }
}