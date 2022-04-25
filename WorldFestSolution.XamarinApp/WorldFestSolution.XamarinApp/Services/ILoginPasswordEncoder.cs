namespace WorldFestSolution.XamarinApp.Services
{
    public interface ILoginPasswordEncoder
    {
        string Encode(string login, string password);
    }
}
