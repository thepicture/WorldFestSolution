using System;
using System.IO;
using System.Text;
using System.Windows.Input;
using WorldFestSolution.XamarinApp.Models.Serialized;
using WorldFestSolution.XamarinApp.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WorldFestSolution.XamarinApp.ViewModels
{
    public class AddEditFestivalViewModel : BaseViewModel
    {
        private Festival festival;

        public AddEditFestivalViewModel(Festival festival)
        {
            Festival = festival;
        }

        public Festival Festival
        {
            get => festival;
            set => SetProperty(ref festival, value);
        }

        private Command saveChangesCommand;

        public ICommand SaveChangesCommand
        {
            get
            {
                if (saveChangesCommand == null)
                {
                    saveChangesCommand = new Command(SaveChangesAsync);
                }

                return saveChangesCommand;
            }
        }

        private async void SaveChangesAsync()
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(Festival.Title))
            {
                _ = errors.AppendLine("Введите название");
            }
            if (Festival.FromDateTime <= DateTime.Now)
            {
                _ = errors.AppendLine("Дата начала фестиваля " +
                    "должна быть позднее текущей даты");
            }
            if (Festival.FestivalProgram.Count == 0)
            {
                _ = errors.AppendLine("Создайте хотя бы одну " +
                    "программу для фестиваля");
            }
            if (errors.Length > 0)
            {
                await AlertService.InformError(
                    errors.ToString());
                return;
            }
            if (await FestivalDataStore.AddItemAsync(Festival))
            {
                await Shell.Current.GoToAsync("..");
            }
        }

        private Command getImageCommand;
        private ImageSource imageSource;

        public ICommand GetImageCommand
        {
            get
            {
                if (getImageCommand == null)
                {
                    getImageCommand = new Command(GetImageAsync);
                }

                return getImageCommand;
            }
        }

        public ImageSource ImageSource
        {
            get => imageSource;
            set => SetProperty(ref imageSource, value);
        }

        private async void GetImageAsync()
        {
            FileResult result = await MediaPicker
               .PickPhotoAsync(new MediaPickerOptions
               {
                   Title = "Выберите фото фестиваля"
               });
            Stream imageStream = await result.OpenReadAsync();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                await imageStream.CopyToAsync(memoryStream);
                Festival.Image = memoryStream.ToArray();
            }
            ImageSource = ImageSource.FromStream(() =>
            {
                return new MemoryStream(Festival.Image);
            });
        }

        private Command goToAddProgramViewCommand;

        public ICommand GoToAddProgramViewCommand
        {
            get
            {
                if (goToAddProgramViewCommand == null)
                {
                    goToAddProgramViewCommand = new Command(GoToAddProgramViewAsync);
                }

                return goToAddProgramViewCommand;
            }
        }

        private async void GoToAddProgramViewAsync()
        {
            await Shell.Current.Navigation.PushAsync(
                new AddProgramView(
                    new AddEditProgramViewModel(Festival.FestivalProgram)));
        }

        private Command<FestivalProgram> goToEditProgramViewCommand;

        public Command<FestivalProgram> GoToEditProgramViewCommand
        {
            get
            {
                if (goToEditProgramViewCommand == null)
                {
                    goToEditProgramViewCommand = new Command<FestivalProgram>(GoToEditProgramViewAsync);
                }

                return goToEditProgramViewCommand;
            }
        }

        private async void GoToEditProgramViewAsync(FestivalProgram program)
        {
            await Shell.Current.Navigation.PushAsync(
                new AddProgramView(
                    new AddEditProgramViewModel(Festival.FestivalProgram, program)));
        }

        private Command<FestivalProgram> deleteProgramCommand;

        public Command<FestivalProgram> DeleteProgramCommand
        {
            get
            {
                if (deleteProgramCommand == null)
                {
                    deleteProgramCommand = new Command<FestivalProgram>(DeleteProgramAsync);
                }

                return deleteProgramCommand;
            }
        }

        private async void DeleteProgramAsync(FestivalProgram program)
        {
            if (await AlertService.Ask($"Удалить программу {program.Title}?"))
            {
                Festival.FestivalProgram.Remove(program);
                await AlertService.Inform($"Программа {program.Title} " +
                    $"будет удалена после подтверждения " +
                    $"изменений фестиваля");
            }
        }
    }
}