using WorldFestSolution.WebAPI.Models.Entities;

namespace WorldFestSolution.WebAPI.Models.Serialized
{
    public class SerializedUser
    {
        public SerializedUser(User user)
        {
            Id = user.Id;
            Login = user.Login;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Patronymic = user.Patronymic;
            UserTypeId = user.UserTypeId;
            Image = user.Image;
        }

        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public int UserTypeId { get; set; }
        public byte[] Image { get; set; }
    }
}