using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
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
        public IAuthenticationService AuthenticationService =>
            DependencyService.Get<IAuthenticationService>();
        public IRegistrationService RegistrationService =>
         DependencyService.Get<IRegistrationService>();
        public IDataStore<Festival> FestivalDataStore =>
             DependencyService.Get<IDataStore<Festival>>();
        public InviteOfFestivalDataStore InviteOfFestivalDataStore =>
           DependencyService.Get<InviteOfFestivalDataStore>();
        public User User => Identity.User;

        private bool isRefreshing = false;
        private bool isBusy = false;
        public bool IsBusy
        {
            get => isBusy;
            set => SetProperty(ref isBusy, value);
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
