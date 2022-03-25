using System.Threading.Tasks;

namespace WorldFestSolution.XamarinApp.Services
{
    public interface IAuthenticationService : IHaveMessage
    {
        Task<bool> AuthenticateAsync(string login, string password);
    }
}
