using System.Threading.Tasks;
using Xamarin.Forms;

namespace WorldFestSolution.XamarinApp.Services
{
    public interface IAlertService
    {
        Task Inform(object information);
        Task<bool> Ask(object question);
        Task Warn(object warning);
        Task InformError(object error);
        Task<string> Prompt(object message, int maxLength, Keyboard keyboard);
    }
}
