using System.Threading.Tasks;
using WorldFestSolution.XamarinApp.Models.Serialized;

namespace WorldFestSolution.XamarinApp.Services
{
    public interface IRegistrationService
    {
        Task<bool> RegisterAsync(User user);
    }
}
