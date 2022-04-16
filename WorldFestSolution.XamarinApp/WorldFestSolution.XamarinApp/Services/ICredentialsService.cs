namespace WorldFestSolution.XamarinApp.Services
{
    public interface ICredentialsService
    {
        string Encode(string login, string password);
        string[] Decode();
    }
}
