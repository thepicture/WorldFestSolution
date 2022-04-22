using System.Threading.Tasks;
using Xamarin.Forms;

namespace WorldFestSolution.XamarinApp.Services
{
    public class AndroidAlertService : IAlertService
    {
        public async Task<bool> Ask(object question)
        {
            return await App
                .Current
                .MainPage
                .DisplayAlert("Вопрос",
                              question.ToString(),
                              "Да",
                              "Нет");
        }

        public async Task Inform(object information)
        {
            await App
            .Current
            .MainPage
            .DisplayAlert("Информация",
                          information.ToString(),
                          "ОК");
        }

        public async Task InformError(object error)
        {
            await App
           .Current
           .MainPage
           .DisplayAlert("Ошибка",
                         error.ToString(),
                         "ОК");
        }

        public async Task<string> Prompt(object message, int maxLength, Keyboard keyboard)
        {
            return await App
                .Current
                .MainPage
                .DisplayPromptAsync("Форма заполнения",
                                    message.ToString(),
                                    accept: "Подтвердить",
                                    cancel: "Отмена",
                                    placeholder: message.ToString(),
                                    maxLength: maxLength,
                                    keyboard: keyboard,
                                    initialValue: "");
        }

        public async Task Warn(object warning)
        {
            await App
            .Current
            .MainPage
            .DisplayAlert("Предупреждение",
                          warning.ToString(),
                          "ОК");
        }
    }
}
