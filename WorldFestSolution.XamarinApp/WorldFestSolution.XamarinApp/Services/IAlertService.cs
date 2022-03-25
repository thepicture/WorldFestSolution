using System.Threading.Tasks;

namespace WorldFestSolution.XamarinApp.Services
{
    public interface IAlertService
    {
        Task Inform(string information);
        Task<bool> Ask(string question);
        Task Warn(string warning);
        Task InformError(string error);
    }
}
