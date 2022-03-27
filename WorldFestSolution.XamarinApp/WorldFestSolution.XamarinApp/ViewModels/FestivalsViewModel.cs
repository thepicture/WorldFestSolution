using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using WorldFestSolution.XamarinApp.Models.Serialized;
using Xamarin.Forms;

namespace WorldFestSolution.XamarinApp.ViewModels
{
    public class FestivalsViewModel : BaseViewModel
    {
        private string searchText;

        public string SearchText
        {
            get => searchText;
            set
            {
                if (SetProperty(ref searchText, value))
                {
                    IsRefreshing = true;
                }
            }
        }

        internal void OnAppearing()
        {
            IsRefreshing = true;
        }

        private ObservableCollection<Festival> festivals;

        public ObservableCollection<Festival> Festivals
        {
            get => festivals;
            set => SetProperty(ref festivals, value);
        }

        private Command refreshCommand;

        public FestivalsViewModel()
        {
            Festivals = new ObservableCollection<Festival>();
        }

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
            IEnumerable<Festival> items = await Task.Run(() =>
            {
                return FestivalDataStore.GetItemsAsync();
            });
            if (items != null)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Festivals.Clear();
                });
                if (!string.IsNullOrWhiteSpace(SearchText))
               {
                    items = items.Where(i =>
                    {
                        return i.Title.IndexOf(SearchText,
                                              StringComparison.OrdinalIgnoreCase) != -1;
                    });
                }
                foreach (Festival festival in items)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Festivals.Add(festival);
                    });
                    await Task.Delay(500);
                }
            }
            IsRefreshing = false;
        }

        private Command goToAddFestivalViewCommand;

        public ICommand GoToAddFestivalViewCommand
        {
            get
            {
                if (goToAddFestivalViewCommand == null)
                {
                    goToAddFestivalViewCommand = new Command(GoToAddFestivalView);
                }

                return goToAddFestivalViewCommand;
            }
        }

        private void GoToAddFestivalView()
        {
        }

        private Command<Festival> goToEditFestivalViewCommand;

        public Command<Festival> GoToEditFestivalViewCommand
        {
            get
            {
                if (goToEditFestivalViewCommand == null)
                {
                    goToEditFestivalViewCommand = new Command<Festival>(GoToEditFestivalView);
                }

                return goToEditFestivalViewCommand;
            }
        }

        private void GoToEditFestivalView(Festival festival)
        {
        }

        private Command<Festival> deleteFestivalCommand;

        public Command<Festival> DeleteFestivalCommand
        {
            get
            {
                if (deleteFestivalCommand == null)
                {
                    deleteFestivalCommand = new Command<Festival>(DeleteFestivalAsync);
                }

                return deleteFestivalCommand;
            }
        }

        private async void DeleteFestivalAsync(Festival festival)
        {
            if (await AlertService.Ask($"Удалить фестиваль {festival.Title}? " +
                "При удалении фестиваля у вас уменьшится рейтинг"))
            {
                if (await FestivalDataStore.DeleteItemAsync(
                    festival.Id.ToString()))
                {
                    await AlertService.Inform($"Фестиваль {festival.Title} удалён. " +
                        "Ваш рейтинг уменьшился");
                    IsRefreshing = true;
                }
            }
        }
    }
}