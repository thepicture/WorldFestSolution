using Newtonsoft.Json;
using System.IO;
using Xamarin.Forms;

namespace WorldFestSolution.XamarinApp.Models.Serialized
{
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

        public double Rating { get; set; }
    }
}
