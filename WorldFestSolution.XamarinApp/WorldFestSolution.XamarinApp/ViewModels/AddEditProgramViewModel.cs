using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using WorldFestSolution.XamarinApp.Models.Serialized;
using Xamarin.Forms;

namespace WorldFestSolution.XamarinApp.ViewModels
{
    public class AddEditProgramViewModel : BaseViewModel
    {
        private ObservableCollection<FestivalProgram> programs;

        public AddEditProgramViewModel(
            ObservableCollection<FestivalProgram> programs,
            FestivalProgram programToEdit = null)
        {
            Programs = programs;
            if (programToEdit != null)
            {
                Program = programToEdit;
            }
            else
            {
                Program.DurationInMinutes = 30;
            }
        }

        public ObservableCollection<FestivalProgram> Programs
        {
            get => programs;
            set => SetProperty(ref programs, value);
        }

        private Command addProgramCommand;

        public ICommand AddProgramCommand
        {
            get
            {
                if (addProgramCommand == null)
                {
                    addProgramCommand = new Command(AddProgramAsync);
                }

                return addProgramCommand;
            }
        }

        private async void AddProgramAsync()
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(Program.Title))
            {
                _ = errors.AppendLine("Введите название");
            }
            if (Program.DurationInMinutes <= 0)
            {
                _ = errors.AppendLine("Введите " +
                    "положительную длительность в минутах");
            }
            if (errors.Length > 0)
            {
                await AlertService.InformError(errors);
                return;
            }
            string action = Program.IsAddedLocally ? "изменена" : "добавлена";
            if (!Program.IsAddedLocally)
            {
                Program.IsAddedLocally = true;
                Programs.Add(Program);
            }
            await AlertService.Inform($"Программа {action} " +
                "и будет сохранена после подтверждения " +
                "изменений фестиваля");
            await Shell.Current.GoToAsync("..");
        }

        private FestivalProgram program = new FestivalProgram();

        public FestivalProgram Program
        {
            get => program;
            set => SetProperty(ref program, value);
        }
    }
}