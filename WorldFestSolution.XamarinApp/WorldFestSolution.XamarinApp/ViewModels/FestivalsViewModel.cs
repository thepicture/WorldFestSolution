using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using WorldFestSolution.XamarinApp.Controls;
using WorldFestSolution.XamarinApp.Models.Filters;
using WorldFestSolution.XamarinApp.Models.Serialized;
using WorldFestSolution.XamarinApp.Views;
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

        public FestivalsViewModel(bool isRelatedToMe)
        {
            Festivals = new ObservableCollection<Festival>();
            IsRelatedToMe = isRelatedToMe;
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
            Festivals.Clear();
            IEnumerable<Festival> items = await Task.Run(() =>
            {
                if (IsRelatedToMe)
                {
                    return UserFestivalDataStore.GetItemAsync("");
                }
                else
                {
                    return FestivalDataStore.GetItemsAsync();
                }
            });
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                items = items.Where(i =>
                {
                    return i
                    .Title
                    .IndexOf(SearchText,
                             StringComparison.OrdinalIgnoreCase) != -1;
                });
            }
            if (IsActualOnly)
            {
                items = items.Where(i => i.IsStarting || i.IsLive);
            }
            items = SelectedFilter.Accept(items);
            foreach (Festival festival in items)
            {
                Festivals.Add(festival);
                await Task.Delay(50);
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
                    goToAddFestivalViewCommand
                        = new Command(GoToAddFestivalViewAsync);
                }

                return goToAddFestivalViewCommand;
            }
        }

        private async void GoToAddFestivalViewAsync()
        {
            await Shell.Current.Navigation.PushAsync(
                new AddEditFestivalView(
                    new AddEditFestivalViewModel(
                        new Festival())));
        }

        private Command<Festival> goToFestivalViewCommand;

        public Command<Festival> GoToFestivalViewCommand
        {
            get
            {
                if (goToFestivalViewCommand == null)
                {
                    goToFestivalViewCommand
                        = new Command<Festival>(GoToFestivalViewAsync);
                }

                return goToFestivalViewCommand;
            }
        }

        private async void GoToFestivalViewAsync(Festival festival)
        {
            await Shell.Current.Navigation.PushAsync(
                new FestivalView(new FestivalViewModel(festival.Id)));
        }

        private Command<Festival> deleteFestivalCommand;

        public Command<Festival> DeleteFestivalCommand
        {
            get
            {
                if (deleteFestivalCommand == null)
                {
                    deleteFestivalCommand
                        = new Command<Festival>(DeleteFestivalAsync);
                }

                return deleteFestivalCommand;
            }
        }

        private async void DeleteFestivalAsync(Festival festival)
        {
            if (await FestivalDataStore.DeleteItemAsync(
                festival.Id.ToString()))
            {
                IsRefreshing = true;
            }
        }

        private bool isActualOnly = true;

        public bool IsActualOnly
        {
            get => isActualOnly;
            set
            {
                if (SetProperty(ref isActualOnly, value))
                {
                    IsRefreshing = true;
                }
            }
        }

        private Command<SelfSendableRatingBar> rateFestivalCommand;

        public Command<SelfSendableRatingBar> RateFestivalCommand
        {
            get
            {
                if (rateFestivalCommand == null)
                {
                    rateFestivalCommand =
                        new Command<SelfSendableRatingBar>(RateFestivalAsync);
                }

                return rateFestivalCommand;
            }
        }

        private async void RateFestivalAsync(SelfSendableRatingBar ratingBar)
        {
            Festival festival = ratingBar.BindingContext as Festival;
            FestivalRating festivalRating = new FestivalRating
            {
                FestivalId = festival.Id,
                CountOfStars = (int)festival.Rating,
                IsRated = festival.IsRated
            };
            await FestivalRatingDataStore.AddItemAsync(festivalRating);
            IsRefreshing = true;
        }

        private IFilter selectedFilter;

        public IFilter SelectedFilter
        {
            get => selectedFilter;
            set
            {
                if (SetProperty(ref selectedFilter, value))
                {
                    IsRefreshing = true;
                }
            }
        }

        public bool IsRelatedToMe { get; }
    }
}