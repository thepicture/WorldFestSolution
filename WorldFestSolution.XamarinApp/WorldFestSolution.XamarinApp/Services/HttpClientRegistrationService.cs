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
                    .InformError(
                    validationErrors.ToString());
                return false;
            }

            string jsonUser = JsonConvert.SerializeObject(user);
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(Api.BaseUrl);
                try
                {
                    HttpResponseMessage response = await client
                     .PostAsync(new Uri(client.BaseAddress + "users/register"),
                                new StringContent(jsonUser,
                                                  Encoding.UTF8,
                                                  "application/json"));
                    if (response.StatusCode == HttpStatusCode.Created)
                    {
                        await DependencyService
                            .Get<IAlertService>()
                            .Inform("Вы зарегистрированы");
                        return true;
                    }
                    else
                    {
                        await DependencyService
                           .Get<IAlertService>()
                           .Inform("Регистрация неуспешна. " +
                            "Попробуйте ещё раз");
                        return false;
                    }
                }
                catch (HttpRequestException ex)
                {
                    await DependencyService
                           .Get<IAlertService>()
                           .InformError("Ошибка запроса: "
                           + ex.StackTrace);
                    Debug.WriteLine(ex.StackTrace);
                }
                catch (TaskCanceledException ex)
                {
                    await DependencyService
                           .Get<IAlertService>()
                           .InformError("Запрос отменён: "
                           + ex.StackTrace);
                    Debug.WriteLine(ex.StackTrace);
                }
                catch (InvalidOperationException ex)
                {
                    await DependencyService
                           .Get<IAlertService>()
                           .InformError("Операция некорректна: "
                           + ex.StackTrace);
                    Debug.WriteLine(ex.StackTrace);
                }
                catch (Exception ex)
                {
                    await DependencyService
                           .Get<IAlertService>()
                           .InformError("Неизвестная ошибка: "
                           + ex.StackTrace);
                    Debug.WriteLine(ex.StackTrace);
                }
            }
            return false;
        }
    }
}
