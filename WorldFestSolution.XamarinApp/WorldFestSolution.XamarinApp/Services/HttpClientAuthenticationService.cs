using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WorldFestSolution.XamarinApp.Models;
using Xamarin.Forms;

namespace WorldFestSolution.XamarinApp.Services
{
    public class HttpClientAuthenticationService : IAuthenticationService
    {
        public string Message { get; set; }

        public async Task<bool> AuthenticateAsync(string login, string password)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(login))
            {
                _ = errors.AppendLine("Введите логин");
            }
            if (string.IsNullOrWhiteSpace(password))
            {
                _ = errors.AppendLine("Введите пароль");
            }

            if (errors.Length > 0)
            {
                await DependencyService
                    .Get<IAlertService>()
                    .InformError(
                        errors.ToString());
                return false;
            }
            string encodedLoginAndPassword = DependencyService
                .Get<ICredentialsService>()
                .Encode(login, password);
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization =
                     new AuthenticationHeaderValue("Basic",
                                                   encodedLoginAndPassword);
                client.BaseAddress = new Uri(Api.BaseUrl);
                try
                {
                    HttpResponseMessage response = await client
                        .GetAsync("users/authenticate");
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        string content = await response.Content
                            .ReadAsStringAsync();
                        Message = content;
                        return true;
                    }
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        Message = "Вы ввели "
                            + "неверный логин или пароль. "
                            + "Попробуйте ещё раз";
                    }
                    else
                    {
                        Message = "Произошла ошибка подключения";
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