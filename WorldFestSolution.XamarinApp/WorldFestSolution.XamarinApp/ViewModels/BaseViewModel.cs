using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using WorldFestSolution.XamarinApp.Models;
using WorldFestSolution.XamarinApp.Models.Serialized;
using WorldFestSolution.XamarinApp.Services;
using Xamarin.Forms;

namespace WorldFestSolution.XamarinApp.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public IAlertService AlertService =>
            DependencyService.Get<IAlertService>();
        public IDataStore<LoginUser> LoginUserDataStore =>
            DependencyService.Get<IDataStore<LoginUser>>();
        public IRegistrationService RegistrationService =>
            DependencyService.Get<IRegistrationService>();
        public IDataStore<Festival> FestivalDataStore =>
            DependencyService.Get<IDataStore<Festival>>();
        public IDataStore<Comment> CommentDataStore =>
            DependencyService.Get<IDataStore<Comment>>();
        public IDataStore<UserOfFestival> UserOfFestivalDataStore =>
            DependencyService.Get<IDataStore<UserOfFestival>>();
        public IDataStore<ChangePasswordCredentials> ChangePasswordDataStore =>
            DependencyService.Get<IDataStore<ChangePasswordCredentials>>();
        public IDataStore<FestivalRating> FestivalRatingDataStore =>
            DependencyService.Get<IDataStore<FestivalRating>>();
        public IDataStore<ResponseInvite> InviteDataStore =>
            DependencyService.Get<IDataStore<ResponseInvite>>();
        public IDataStore<IEnumerable<Comment>> FestivalCommentDataStore =>
            DependencyService.Get<IDataStore<IEnumerable<Comment>>>();
        public IDataStore<IEnumerable<ResponseInvite>> FestivalResponseInviteDataStore =>
            DependencyService.Get<IDataStore<IEnumerable<ResponseInvite>>>();
        public IDataStore<byte[]> UserImageDataStore =>
            DependencyService.Get<IDataStore<byte[]>>();
        public IDataStore<FestivalPopularity> FestivalPopularityDataStore =>
            DependencyService.Get<IDataStore<FestivalPopularity>>();
        public IDataStore<User> UserDataStore =>
            DependencyService.Get<IDataStore<User>>();
        public IDataStore<IEnumerable<Festival>> UserFestivalDataStore =>
            DependencyService.Get<IDataStore<IEnumerable<Festival>>>();
        public IDataStore<UserRating> UserRatingDataStore =>
            DependencyService.Get<IDataStore<UserRating>>();
        public IDataStore<IEnumerable<ParticipantUser>> FestivalUsersDataStore =>
            DependencyService.Get<IDataStore<IEnumerable<ParticipantUser>>>();
        public ILoginPasswordEncoder LoginPasswordEncoder =>
            DependencyService.Get<ILoginPasswordEncoder>();
        public IImageTransformService ImageTransformService =>
            DependencyService.Get<IImageTransformService>();
        public User User
        {
            get => Identity.User;
            set
            {
                Identity.User = value;
                OnPropertyChanged();
            }
        }

        private bool isRefreshing = false;
        private bool isBusy = false;
        public bool IsBusy
        {
            get => isBusy;
            set
            {
                if (SetProperty(ref isBusy, value))
                {
                    OnPropertyChanged(nameof(IsBusy));
                }
            }
        }

        public bool IsNotBusy => !IsBusy;

        private string title = string.Empty;
        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        public bool IsRefreshing
        {
            get => isRefreshing;
            set => SetProperty(ref isRefreshing, value);
        }


        private Command changeApiUrlCommand;

        public ICommand ChangeApiUrlCommand
        {
            get
            {
                if (changeApiUrlCommand == null)
                    changeApiUrlCommand = new Command(ChangeApiUrlAsync);

                return changeApiUrlCommand;
            }
        }

        private async void ChangeApiUrlAsync()
        {
            string newBaseUrl = await App.Current.MainPage
                   .DisplayPromptAsync("Изменить путь к API",
                                       "Введите путь к API",
                                       initialValue: Api.BaseUrl);
            Models.Api.BaseUrl = newBaseUrl;
        }


        private Command exitCommand;

        public ICommand ExitCommand
        {
            get
            {
                if (exitCommand == null)
                {
                    exitCommand = new Command(ExitAsync);
                }

                return exitCommand;
            }
        }

        private async void ExitAsync()
        {
            if (await AlertService.Ask("Выключить приложение?"))
            {
                Environment.Exit(0);
            }
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
            {
                return false;
            }

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
