using System.Collections.ObjectModel;
using System.Text;
using WorldFestSolution.XamarinApp.Models.Serialized;
using Xamarin.Forms;

namespace WorldFestSolution.XamarinApp.ViewModels
{
    [PropertyChanged.AddINotifyPropertyChangedInterface]
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
                Program.DurationInMinutesAsString = 30.ToString();
            }
            Program.PropertyChanged += (_, __) =>
            {
                AddProgramCommand?.ChangeCanExecute();
            };
        }

        public ObservableCollection<FestivalProgram> Programs
        {
            get => programs;
            set => SetProperty(ref programs, value);
        }

        private Command addProgramCommand;

        public Command AddProgramCommand
        {
            get
            {
                if (addProgramCommand == null)
                {
                    addProgramCommand = new Command(AddProgramAsync, () =>
                    {
                        return !Program.IsHasTitleError && !Program.IsHasDurationInMinutesError;
                    });
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