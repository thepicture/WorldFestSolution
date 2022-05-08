using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WorldFestSolution.XamarinApp.Models;
using WorldFestSolution.XamarinApp.Models.Serialized;
using Xamarin.Forms;

namespace WorldFestSolution.XamarinApp.Services
{
    public class HttpClientRegistrationService : IRegistrationService
    {
        public async Task<bool> RegisterAsync(User user)
        {
            StringBuilder validationErrors = new StringBuilder();
            if (user.UserTypeId == 0)
            {
                _ = validationErrors.AppendLine("Укажите роль");
            }
            if (string.IsNullOrWhiteSpace(user.Is18OrMoreYearsOldAsString))
            {
                _ = validationErrors.AppendLine("Укажите ваш возраст");
            }
            if (string.IsNullOrWhiteSpace(user.Login))
            {
                _ = validationErrors.AppendLine("Введите логин");
            }
            if (string.IsNullOrWhiteSpace(user.Password))
            {
                _ = validationErrors.AppendLine("Введите пароль");
            }
            if (string.IsNullOrWhiteSpace(user.LastName))
            {
                _ = validationErrors.AppendLine("Введите фамилию");
            }
            if (string.IsNullOrWhiteSpace(user.FirstName))
            {
                _ = validationErrors.AppendLine("Введите имя");
            }

            if (validationErrors.Length > 0)
            {
                await DependencyService
                    .Get<IAlertService>()
                    .InformError(validationErrors);
                return false;
            }

            user.Is18OrMoreYearsOld =
                user.Is18OrMoreYearsOldAsString == "Мне уже есть 18";

            string jsonUser = JsonConvert.SerializeObject(user);
            using (HttpClient client = new HttpClient(App.ClientHandler))
            {
                client.BaseAddress = new Uri(Api.BaseUrl);
                try
                {
                    HttpResponseMessage response = await client
                     .PostAsync("users/register",
                                new StringContent(jsonUser,
                                                  Encoding.UTF8,
                                                  "application/json"));
                    if (response.StatusCode == HttpStatusCode.Created)
                    {
                        await DependencyService
                            .Get<IAlertService>()
                            .Inform("Вы зарегистрированы");
                    }
                    else
                    {
                        Debug.WriteLine(response);
                        await DependencyService
                            .Get<IAlertService>()
                            .InformError(response);
                    }
                    return response.StatusCode == HttpStatusCode.Created;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    await DependencyService
                        .Get<IAlertService>()
                        .InformError(ex);
                    return false;
                }
            }
        }
    }
}
