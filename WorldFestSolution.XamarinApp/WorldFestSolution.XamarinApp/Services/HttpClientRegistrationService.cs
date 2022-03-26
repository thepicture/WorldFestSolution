using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WorldFestSolution.XamarinApp.Models;
using WorldFestSolution.XamarinApp.Models.Serialized;

namespace WorldFestSolution.XamarinApp.Services
{
    public class HttpClientRegistrationService : IRegistrationService
    {
        public string Message { get; set; }

        public async Task<bool> RegisterAsync(User user)
        {
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
                        Message = "Вы зарегистрированы";
                        return true;
                    }
                    else
                    {
                        Message = "Регистрация неуспешна. " +
                            "Попробуйте ещё раз";
                        return false;
                    }
                }
                catch (HttpRequestException ex)
                {
                    Message = "Ошибка запроса: " + ex.StackTrace;
                    Debug.WriteLine(ex.StackTrace);
                }
                catch (TaskCanceledException ex)
                {
                    Message = "Запрос отменён: " + ex.StackTrace;
                    Debug.WriteLine(ex.StackTrace);
                }
                catch (InvalidOperationException ex)
                {
                    Message = "Операция некорректна: " + ex.StackTrace;
                    Debug.WriteLine(ex.StackTrace);
                }
                catch (Exception ex)
                {
                    Message = "Неизвестная ошибка: " + ex.StackTrace;
                    Debug.WriteLine(ex.StackTrace);
                }
            }
            return false;
        }
    }
}
