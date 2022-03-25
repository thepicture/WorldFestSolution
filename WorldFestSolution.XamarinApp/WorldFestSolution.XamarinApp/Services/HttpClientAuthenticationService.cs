using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WorldFestSolution.XamarinApp.Models;

namespace WorldFestSolution.XamarinApp.Services
{
    public class HttpClientAuthenticationService : IAuthenticationService
    {
        public string Message { get; set; }

        public async Task<bool> AuthenticateAsync(string login, string password)
        {
            string encodedLoginAndPassword =
           CredentialsToBasicConverter
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
                    if (response.StatusCode != HttpStatusCode.Unauthorized)
                    {
                        string content = await response.Content
                            .ReadAsStringAsync();
                        Message = content.Replace("\"", "");
                        return true;
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