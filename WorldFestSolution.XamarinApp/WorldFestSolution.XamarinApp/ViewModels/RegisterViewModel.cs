using System.IO;
using System.Windows.Input;
using WorldFestSolution.XamarinApp.Models.Serialized;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WorldFestSolution.XamarinApp.ViewModels
{
    public class RegisterViewModel : BaseViewModel
    {
        private UserType userType;

        public UserType UserType
        {
            get => userType;
            set => SetProperty(ref userType, value);
        }

        private string login;

        public string Login
        {
            get => login;
            set => SetProperty(ref login, value);
        }

        private string password;

        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }

        private Command registerCommand;

        public ICommand RegisterCommand
        {
            get
            {
                if (registerCommand == null)
                {
                    registerCommand = new Command(RegisterAsync);
                }

                return registerCommand;
            }
        }

        private async void RegisterAsync()
        {
            User user = new User
            {
                Login = Login,
                Password = Password,
                FirstName = FirstName,
                LastName = LastName,
                Patronymic = Patronymic,
                UserTypeId = UserType == null ? 0 : UserType.Id,
                Image = ImageBytes
            };

            IsBusy = true;
            IsRefreshing = true;
            if (await RegistrationService.RegisterAsync(user))
            {
                (AppShell.Current as AppShell).LoadLoginAndRegisterShell();
            }
            IsRefreshing = false;
            IsBusy = false;
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

        private void RefreshAsync()
        {
            if (IsNotBusy)
            {
                IsRefreshing = false;
            }
        }

        private Command getImageCommand;

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

        private async void GetImageAsync()
        {
            FileResult result = await MediaPicker
                .PickPhotoAsync(new MediaPickerOptions
                {
                    Title = "Выберите фото профиля"
                });
            Stream imageStream = await result.OpenReadAsync();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                await imageStream.CopyToAsync(memoryStream);
                ImageBytes = memoryStream.ToArray();
            }
            Image = ImageSource.FromStream(() =>
            {
                return new MemoryStream(ImageBytes);
            });
        }

        private string lastName;

        public string LastName
        {
            get => lastName;
            set => SetProperty(ref lastName, value);
        }

        private string firstName;

        public string FirstName
        {
            get => firstName;
            set => SetProperty(ref firstName, value);
        }

        private string patronymic;

        public string Patronymic
        {
            get => patronymic;
            set => SetProperty(ref patronymic, value);
        }

        private ImageSource image;
        private byte[] imageBytes;

        public ImageSource Image
        {
            get => image;
            set => SetProperty(ref image, value);
        }
        public byte[] ImageBytes
        {
            get => imageBytes;
            set => SetProperty(ref imageBytes, value);
        }
    }
}