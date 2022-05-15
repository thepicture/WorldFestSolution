namespace WorldFestSolution.XamarinApp.Models.Serialized
{
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class RegistrationUser : User
    {
        public bool IsPasswordHasErrors => string.IsNullOrWhiteSpace(Password);
        public string PasswordErrorText => "Введите пароль";
        public bool IsFirstNameHasErrors => string.IsNullOrWhiteSpace(FirstName);
        public string FirstNameErrorText => "Введите имя";
        public bool IsLastNameHasErrors => string.IsNullOrWhiteSpace(LastName);
        public string LastNameErrorText => "Введите фамилию";
        public bool IsLoginHasErrors => string.IsNullOrWhiteSpace(Login);
        public string LoginErrorText => "Введите логин";
    }
}
