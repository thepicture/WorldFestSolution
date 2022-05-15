using Newtonsoft.Json;
using System.ComponentModel;
using System.IO;
using Xamarin.Forms;

namespace WorldFestSolution.XamarinApp.Models.Serialized
{
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public int UserTypeId { get; set; }
        public byte[] Image { get; set; }
        [JsonIgnore]
        public ImageSource ImageSource
        {
            get
            {
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
        [JsonIgnore]
        public string Is18OrMoreYearsOldAsString { get; set; }
    }
}
