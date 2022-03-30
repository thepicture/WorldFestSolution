using System.Threading.Tasks;
using Xamarin.Forms;

namespace WorldFestSolution.XamarinApp.Services
{
    public interface IAlertService
    {
        Task Inform(string information);
        Task<bool> Ask(string question);
        Task Warn(string warning);
        Task InformError(string error);
        Task<string> Prompt(string message, int maxLength, Keyboard keyboard);
    }
}
