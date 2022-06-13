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


        public FestivalsViewModel(bool isRelatedToMe)
        {
            Festivals = new ObservableCollection<Festival>();
            IsRelatedToMe = isRelatedToMe;
        }

        /// <summary>
        /// Фоновая переменная для хранения 
        /// текста для поиска фестиваля.
        /// </summary>
        private string searchText;

        /// <summary>
        /// Свойство, оборачивающее 
        /// фоновую переменную 
        /// для хранения 
        /// текста для поиска фестиваля.
        /// </summary>
        public string SearchText
        {
            get => searchText;
            set
            {
                // Если текст изменился, 
                if (SetProperty(ref searchText, value))
                {
                    // то вызвать обновление страницы.
                    IsRefreshing = true;
                }
            }
        }

        /// <summary>
        /// Фоновая переменная для хранения 
        /// команды, вызывающейся при обновлении 
        /// модели представления.
        /// </summary>
        private Command refreshCommand;

        /// <summary>
        /// Свойство, оборачивающее 
        /// команду, вызывающуюся при обновлении 
        /// модели представления.
        /// </summary>
        public ICommand RefreshCommand
        {
            // Геттер для инициализации фоновой переменной
            get
            {
                if (refreshCommand == null)
                {
                    refreshCommand = new Command(RefreshAsync);
                }

                return refreshCommand;
            }
        }

        /// <summary>
        /// Обновляет модель представления, 
        /// применяя фильтры для поиска фестиваля.
        /// </summary>
        private async void RefreshAsync()
        {
            // Убрать старый набор элементов.
            Festivals.Clear();

            IEnumerable<Festival> items;
            // Если относится к пользователю,
            if (IsRelatedToMe)
            {
                // То получить фестивали пользователя.
                items = await UserFestivalDataStore.GetItemAsync("");
            }
            else
            {
                // Иначе получить общий набор фестивалей.
                items = await FestivalDataStore.GetItemsAsync();
            }

            // Если текст поиска
            // по названию фестиваля непустой,
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                // то отфильтровать фестивали.
                items = items.Where(i =>
                {
                    return i
                    .Title
                    .IndexOf(SearchText,
                             StringComparison.OrdinalIgnoreCase) != -1;
                });
            }
            // Если требуются фестивали для совершеннолетних, 
            // то отфильтровать выборку.
            if (IsForMaturePeopleOnly)
            {
                items = items.Where(i => !i.IsMinorPeopleAllowed);
            }
            // Если пользователь несовершеннолетний, 
            // то отфильтровать выборку.
            if (!User.Is18OrMoreYearsOld)
            {
                items = items.Where(i => i.IsMinorPeopleAllowed);
            }
            // Если требуются актуальные фестивали, 
            // то отфильтровать выборку.
            if (IsActualOnly)
            {
                items = items.Where(i => i.IsStarting || i.IsLive);
            }
            // Применить фильтр, если он существует. 
            // Здесь применена инверсия зависимостей
            // для избегания switch-конструкций 
            // и if-else-if-конструкций.
            items = SelectedFilter.Accept(items);

            // Для каждого фестиваля
            // в отфильтрованных фестивалях
            foreach (Festival festival in items)
            {
                // Добавить фестиваль
                // в оповещаемую коллекцию.
                Festivals.Add(festival);
                // Создать искуственную задержку 
                // для отсутствия подтормаживания 
                // интерфейса.
                await Task.Delay(50);
            }

            // Выключить обновление модели представления.
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
            Festival addedFestival = new Festival();
            AddEditFestivalViewModel addedFestivalViewModel = new AddEditFestivalViewModel(addedFestival);
            AddEditFestivalView addedFestivalView = new AddEditFestivalView(addedFestivalViewModel);
            await Shell.Current.Navigation.PushAsync(addedFestivalView);
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
                new FestivalView(
                    new FestivalViewModel(festival.Id)));
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

        private Command<ExtendedRatingBar> rateFestivalCommand;

        public Command<ExtendedRatingBar> RateFestivalCommand
        {
            get
            {
                if (rateFestivalCommand == null)
                {
                    rateFestivalCommand =
                        new Command<ExtendedRatingBar>(RateFestivalAsync);
                }

                return rateFestivalCommand;
            }
        }

        private async void RateFestivalAsync(ExtendedRatingBar ratingBar)
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

        private bool isForMaturePeopleOnly;

        public bool IsForMaturePeopleOnly
        {
            get => isForMaturePeopleOnly;
            set
            {
                if (SetProperty(ref isForMaturePeopleOnly, value))
                {
                    IsRefreshing = true;
                }
            }
        }
    }
}