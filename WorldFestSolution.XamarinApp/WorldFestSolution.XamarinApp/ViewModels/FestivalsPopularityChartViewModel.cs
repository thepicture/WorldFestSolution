using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using WorldFestSolution.XamarinApp.Models.Filters;
using WorldFestSolution.XamarinApp.Models.Serialized;

namespace WorldFestSolution.XamarinApp.ViewModels
{
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class FestivalsPopularityChartViewModel : BaseViewModel
    {
        private IFilter filter;

        public ObservableCollection<FestivalPopularity> FestivalPopularities { get; set; }
        public IFilter Filter
        {
            get => filter;
            set
            {
                if (SetProperty(ref filter, value))
                {
                    LoadEntriesAsync();
                }
            }
        }

        private async void LoadEntriesAsync()
        {
            FestivalPopularities?.Clear();
            IEnumerable<FestivalPopularity> currentFestivals
                = await FestivalPopularityDataStore.GetItemsAsync();
            if (Filters == null)
            {
                LoadFilters(currentFestivals);
            }
            if (Filter != null)
            {
                currentFestivals = Filter.Accept(currentFestivals);
            }
            FestivalPopularities =
                new ObservableCollection<FestivalPopularity>(currentFestivals);
        }

        private void LoadFilters(IEnumerable<FestivalPopularity> currentFestivals)
        {
            Filters = new ObservableCollection<IFilter>();
            int countOfFestivals = currentFestivals.Count();
            for (int i = 1; i <= countOfFestivals; i += 5)
            {
                Filters.Add(new FromToFilter
                {
                    Title = $"с {i} по {i + 4}",
                    From = i,
                    To = i + 4,
                    SortType = SortType.Descending,
                    PropertyName = nameof(FestivalPopularity.CountOfUsers)
                });
            }
            if (Filter == null)
            {
                Filter = Filters.FirstOrDefault();
            }
        }

        internal void OnAppearing()
        {
            LoadEntriesAsync();
        }

        public ObservableCollection<IFilter> Filters { get; set; }
    }
}