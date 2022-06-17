using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using WorldFestSolution.XamarinApp.Models;
using WorldFestSolution.XamarinApp.Models.Serialized;

namespace WorldFestSolution.XamarinApp.ViewModels
{
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class UsersPopularityViewModel : BaseViewModel
    {
        public ObservableCollection<User> UsersPopularities { get; set; }

        private async void LoadEntriesAsync()
        {
            UsersPopularities?.Clear();
            IEnumerable<User> currentUsers
                = await UserDataStore.GetItemsAsync();
            if (Identity.IsOrganizer)
            {
                currentUsers = currentUsers.Where(u => u.UserTypeId == 1);
            }
            else
            {
                currentUsers = currentUsers.Where(u => u.UserTypeId == 2);
            }
            UsersPopularities =
                new ObservableCollection<User>(
                    currentUsers.OrderByDescending(t => t.Rating));
        }

        internal void OnAppearing()
        {
            LoadEntriesAsync();
        }
    }
}