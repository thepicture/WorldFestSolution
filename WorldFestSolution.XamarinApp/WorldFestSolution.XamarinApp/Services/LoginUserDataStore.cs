using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WorldFestSolution.XamarinApp.Models;
using WorldFestSolution.XamarinApp.Models.Serialized;
using Xamarin.Forms;

namespace WorldFestSolution.XamarinApp.Services
{
    public class LoginUserDataStore : IDataStore<LoginUser>
    {
        public async Task<bool> AddItemAsync(LoginUser item)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(item.Login))
            {
                _ = errors.AppendLine("Введите логин");
            }
            if (string.IsNullOrWhiteSpace(item.Password))
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
            using (HttpClient client = new HttpClient(App.ClientHandler))
            {
                client.BaseAddress = new Uri(Api.BaseUrl);
                try
                {
                    string userContent = JsonConvert.SerializeObject(item);
                    StringContent content = new StringContent(userContent,
                                                              Encoding.UTF8,
                                                              App.Json);
                    HttpResponseMessage response = await client
                        .PostAsync("users/login", content);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        User user = JsonConvert.DeserializeObject<User>(
                            await response.Content.ReadAsStringAsync());
                        string encodedLoginAndPassword =
                            DependencyService
                                .Get<ILoginPasswordEncoder>()
                                .Encode(item.Login, item.Password);
                        if (item.IsRememberMe)
                        {
                            Identity.User = user;
                            Identity.AuthorizationValue = encodedLoginAndPassword;
                        }
                        else
                        {
                            App.User = user;
                            App.Role = user.UserTypeId == 1 ? "Организатор" : "Участник";
                            App.AuthorizationValue = encodedLoginAndPassword;
                        }
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
                        await DependencyService
                            .Get<IAlertService>()
                            .InformError(response);
                    }
                    return response.StatusCode == HttpStatusCode.OK;
                }
                catch (Exception ex)
                {
                    await DependencyService
                        .Get<IAlertService>()
                        .InformError(ex);
                    return false;
                }
            }
        }

        public Task<bool> DeleteItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<LoginUser> GetItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<LoginUser>> GetItemsAsync(bool forceRefresh = false)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateItemAsync(LoginUser item)
        {
            throw new NotImplementedException();
        }
    }
}
