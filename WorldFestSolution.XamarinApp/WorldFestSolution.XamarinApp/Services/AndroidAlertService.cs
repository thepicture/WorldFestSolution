using System.Threading.Tasks;

namespace WorldFestSolution.XamarinApp.Services
{
    public class AndroidAlertService : IAlertService
    {
        public async Task<bool> Ask(string question)
        {
            return await App
                .Current
                .MainPage
                .DisplayAlert("Вопрос", question, "Да", "Нет");
        }

        public async Task Inform(string information)
        {
            await App
            .Current
            .MainPage
            .DisplayAlert("Информация", information, "ОК");
        }

        public async Task InformError(string error)
        {
            await App
           .Current
           .MainPage
           .DisplayAlert("Ошибка", error, "ОК");
        }

        public async Task Warn(string warning)
        {
            await App
            .Current
            .MainPage
            .DisplayAlert("Предупреждение", warning, "ОК");
        }
    }
}
