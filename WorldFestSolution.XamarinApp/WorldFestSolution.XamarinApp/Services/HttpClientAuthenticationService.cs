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
                    .InformError(errors);
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
                        Message = await response.Content
                            .ReadAsStringAsync();
                        await DependencyService
                            .Get<IAlertService>()
                            .Inform("Вы авторизованы");
                        return true;
                    }
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        await DependencyService
                            .Get<IAlertService>()
                            .InformError("Неверный логин или пароль");
                    }
                    else
                    {
                        Debug.WriteLine(response);
                        await DependencyService
                            .Get<IAlertService>()
                            .InformError(response);
                    }
                    return response.StatusCode == HttpStatusCode.OK;
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