using System.IO;
using System.Windows.Input;
using WorldFestSolution.XamarinApp.Models;
using WorldFestSolution.XamarinApp.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WorldFestSolution.XamarinApp.ViewModels
{
    public class AccountViewModel : BaseViewModel
    {

        private Command logoutCommand;

        public ICommand LogoutCommand
        {
            get
            {
                if (logoutCommand == null)
                {
                    logoutCommand = new Command(LogoutAsync);
                }

                return logoutCommand;
            }
        }

        internal void OnAppearing()
        {
            IsRefreshing = true;
        }

        private async void LogoutAsync()
        {
            if (await AlertService.Ask("Выйти из аккаунта?"))
            {
                Identity.Logout();
                AppShell.LoadLoginAndRegisterShell();
            }
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

        private ImageSource imageSource;

        private async void RefreshAsync()
        {
            byte[] currentImage = await UserImageDataStore.GetItemAsync("");
            if (currentImage != null)
            {
                ImageSource = ImageSource.FromStream(() =>
                {
                    return new MemoryStream(currentImage);
                });
            }
            IsRefreshing = false;
        }

        private Command goToChangePasswordViewCommand;

        public ICommand GoToChangePasswordViewCommand
        {
            get
            {
                if (goToChangePasswordViewCommand == null)
                {
                    goToChangePasswordViewCommand =
                        new Command(GoToChangePasswordView);
                }

                return goToChangePasswordViewCommand;
            }
        }

        public ImageSource ImageSource
        {
            get => imageSource;
            set => SetProperty(ref imageSource, value);
        }

        private async void GoToChangePasswordView()
        {
            await Shell.Current.Navigation.PushAsync(
                new ChangePasswordView());
        }

        private Command goToMyInvitesViewCommand;

        public ICommand GoToMyInvitesViewCommand
        {
            get
            {
                if (goToMyInvitesViewCommand == null)
                {
                    goToMyInvitesViewCommand =
                        new Command(GoToMyInvitesViewAsync);
                }

                return goToMyInvitesViewCommand;
            }
        }

        private async void GoToMyInvitesViewAsync()
        {
            await Shell.Current.Navigation.PushAsync(
               new InviteView(
                   new InviteViewModel(festivalId: 0)));
        }

        private Command changeImageCommand;

        public ICommand ChangeImageCommand
        {
            get
            {
                if (changeImageCommand == null)
                {
                    changeImageCommand = new Command(ChangeImageAsync);
                }

                return changeImageCommand;
            }
        }

        private async void ChangeImageAsync()
        {
            FileResult imageResult = await MediaPicker
                .PickPhotoAsync(new MediaPickerOptions
                {
                    Title = "Выберите новое фото"
                });
            if (imageResult == null)
            {
                await AlertService.Warn("Изменение фото отменено");
                return;
            }

            Stream imageStream = await imageResult.OpenReadAsync();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                await imageStream.CopyToAsync(memoryStream);
                if (await UserImageDataStore.AddItemAsync(
                    memoryStream.ToArray()))
                {
                    IsRefreshing = true;
                }
            }
        }
    }
}