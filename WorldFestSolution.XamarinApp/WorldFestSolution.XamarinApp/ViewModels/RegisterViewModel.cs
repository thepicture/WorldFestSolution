using System.IO;
using System.Windows.Input;
using WorldFestSolution.XamarinApp.Models.Serialized;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WorldFestSolution.XamarinApp.ViewModels
{
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class RegisterViewModel : BaseViewModel
    {
        public RegistrationUser RegistrationUser { get; set; }

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
            IsRefreshing = true;
            if (await RegistrationUserDataStore.AddItemAsync(RegistrationUser))
            {
                AppShell.LoadLoginAndRegisterShell();
            }
            IsRefreshing = false;
        }

        private Command getImageCommand;

        public RegisterViewModel()
        {
            RegistrationUser = new RegistrationUser();
        }

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
            if (result == null) return;
            Stream imageStream = await result.OpenReadAsync();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                await imageStream.CopyToAsync(memoryStream);
                RegistrationUser.Image = ImageTransformService
                    .Transform(
                        memoryStream.ToArray(),
                        App.ImageWidth,
                        App.ImageHeight,
                        App.ImageQuality);
            }
        }
    }
}