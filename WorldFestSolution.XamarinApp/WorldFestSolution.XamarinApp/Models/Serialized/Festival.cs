using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xamarin.Forms;

namespace WorldFestSolution.XamarinApp.Models.Serialized
{
    public class Festival
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public System.DateTime FromDateTime { get; set; }
        public byte[] Image { get; set; }
        public int CountOfComments { get; set; }
        public int CountOfPrograms { get; set; }
        public int CountOfUsers { get; set; }
        public double Rating { get; set; }
        public int OrganizerId { get; set; }
        public string OrganizerFullName { get; set; }
        public IEnumerable<int> UsersId { get; set; }
        public IEnumerable<int> CommentsId { get; set; }
        public bool IsActual { get; set; }
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
        public bool IsMeParticipating => UsersId.Any(ui => ui == Identity.User.Id);
    }
}