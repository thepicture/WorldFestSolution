using Newtonsoft.Json;
using System.IO;
using Xamarin.Forms;

namespace WorldFestSolution.XamarinApp.Models.Serialized
{
    public class Comment
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Text { get; set; }
        public System.DateTime CreationDateTime { get; set; }
        public int FestivalId { get; set; }
        public string UserFullName { get; set; }
        public byte[] UserImage { get; set; }
        public string UserLogin { get; set; }
        [JsonIgnore]
        public ImageSource UserImageSource
        {
            get
            {
                if (UserImage == null)
                {
                    return null;
                }
                else
                {
                    return ImageSource.FromStream(() =>
                    {
                        return new MemoryStream(UserImage);
                    });
                }
            }
        }
    }
}