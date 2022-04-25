using Microcharts;
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
    public class FestivalsPopularityChartViewModel : BaseViewModel
    {
        private Chart chart;
        private ObservableCollection<ChartEntry> entries;
        private readonly Random random = new Random();

        public FestivalsPopularityChartViewModel()
        {
            Entries = new ObservableCollection<ChartEntry>();
            Chart = new BarChart
            {
                ValueLabelOrientation = Orientation.Horizontal,
                LabelTextSize = 50,
                LabelOrientation = Orientation.Horizontal,
                Entries = Entries,
            };
        }

        private async Task LoadEntriesAsync()
        {
            Entries.Clear();
            IsRefreshing = true;
            IEnumerable<FestivalPopularity> festivals
                = await FestivalPopularityDataStore.GetItemsAsync();
            IEnumerable<ChartEntry> festivalEntries = festivals.Select(f =>
               {
                   return new ChartEntry(f.CountOfUsers)
                   {
                       Label = f.FestivalTitle,
                       ValueLabel = f.CountOfUsers.ToString(),
                       Color = SkiaSharp.SKColor.Parse("#"
                                       + random
                                           .Next(200, 256)
                                           .ToString("x2")
                                       + random
                                           .Next(200, 256)
                                           .ToString("x2")
                                       + random
                                           .Next(200, 256)
                                           .ToString("x2"))
                   };
               });
            foreach (ChartEntry festivalEntry in festivalEntries)
            {
                Entries.Add(festivalEntry);
            }
            Chart.Entries = Entries;
            OnPropertyChanged(
                nameof(Chart));
            EntriesChanged?.Invoke();
            IsRefreshing = false;
        }

        public event Action EntriesChanged;

        internal void OnAppearing()
        {
            IsRefreshing = true;
        }

        public ObservableCollection<ChartEntry> Entries
        {
            get => entries;
            set => SetProperty(ref entries, value);
        }
        public Chart Chart
        {
            get => chart;
            set => SetProperty(ref chart, value);
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
            await LoadEntriesAsync();
        }
    }
}