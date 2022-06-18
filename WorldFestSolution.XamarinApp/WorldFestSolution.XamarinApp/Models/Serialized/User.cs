using Newtonsoft.Json;
using System.IO;
using Xamarin.Forms;

namespace WorldFestSolution.XamarinApp.Models.Serialized
{
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class User : BindableObject
    {
        private string login;
        private string password;
        private string firstName;
        private string lastName;

        public int Id { get; set; }
        public string Login
        {
            get => login;
            set
            {
                login = value;
                OnPropertyChanged(nameof(RegistrationUser.IsLoginHasErrors));
            }
        }
        public string Password
        {
            get => password;
            set
            {
                password = value;
                OnPropertyChanged(nameof(RegistrationUser.IsPasswordHasErrors));
            }
        }
        public string FirstName
        {
            get => firstName;
            set
            {
                firstName = value;
                OnPropertyChanged(nameof(RegistrationUser.IsFirstNameHasErrors));
            }
        }
        public string LastName
        {
            get => lastName;
            set
            {
                lastName = value;
                OnPropertyChanged(nameof(RegistrationUser.IsLastNameHasErrors));
            }
        }
        public string Patronymic { get; set; }
        public int UserTypeId { get; set; }
        public byte[] Image { get; set; }
        [JsonIgnore]
        public ImageSource ImageSource
        {
            get
            {
                if (Image == null)
                {
                    return null;
                }
                return ImageSource.FromStream(() =>
                {
                    return new MemoryStream(Image);
                });
            }
        }
        [JsonIgnore]
        public string FullName => string.IsNullOrWhiteSpace(Patronymic)
            ? $"{LastName} {FirstName}"
            : $"{LastName} {FirstName} {Patronymic}";

        public double Rating { get; set; }
        public bool IsRated { get; set; }
        public bool Is18OrMoreYearsOld { get; set; }
        public bool IsWantsInvites { get; set; }
        public int CountOfFestivals { get; set; }
        public int CountOfComments { get; set; }

        [JsonIgnore]
        public string Type
        {
            get
            {
                if (UserTypeId == 1)
                {
                    return "Участник";
                }
                else
                {
                    return "Организатор";
                }
            }
        }
    }
}
