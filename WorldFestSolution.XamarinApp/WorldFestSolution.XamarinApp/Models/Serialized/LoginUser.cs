using Newtonsoft.Json;

namespace WorldFestSolution.XamarinApp.Models.Serialized
{
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class LoginUser : User
    {
        [JsonIgnore]
        public bool IsRememberMe { get; set; }
    }
}
