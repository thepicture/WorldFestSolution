using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WorldFestSolution.XamarinApp.Models;
using WorldFestSolution.XamarinApp.Models.Serialized;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WorldFestSolution.XamarinApp.Services
{
    public class ChangePasswordDataStore : IDataStore<ChangePasswordCredentials>
    {
        public async Task<bool> AddItemAsync(ChangePasswordCredentials item)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(item.OldPassword))
            {
                _ = errors.AppendLine("Введите старый пароль");
            }
            if (string.IsNullOrWhiteSpace(item.NewPassword))
            {
                _ = errors.AppendLine("Введите новый пароль");
            }
            if (!string.IsNullOrWhiteSpace(item.OldPassword)
                && !string.IsNullOrWhiteSpace(item.NewPassword)
                && item.OldPassword == item.NewPassword)
            {
                _ = errors.AppendLine("Новый пароль " +
                    "должен отличаться от старого пароля");
            }
            if (errors.Length > 0)
            {
                await DependencyService
                    .Get<IAlertService>()
                    .InformError(errors);
                return false;
            }
            string jsonPasswordCredentials = JsonConvert.SerializeObject(item);
            using (HttpClient client = new HttpClient(App.ClientHandler))
            {
                client.BaseAddress = new Uri(Api.BaseUrl);
                try
                {
                    HttpResponseMessage response = await client
                     .PostAsync("users/changePassword",
                                new StringContent(jsonPasswordCredentials,
                                                  Encoding.UTF8,
                                                  "application/json"));
                    if (response.StatusCode == HttpStatusCode.Created)
                    {
                        User user = Identity.User ?? App.User;
                        string authorizationValue = DependencyService
                                .Get<ILoginPasswordEncoder>()
                                .Encode(item.Login, item.NewPassword);
                        user.Password = item.NewPassword;
                        if (await SecureStorage.GetAsync("User") is string)
                        {
                            Identity.User = user;
                            Identity.AuthorizationValue = authorizationValue;
                        }
                        else
                        {
                            App.User = user;
                            App.AuthorizationValue = authorizationValue;
                        }
                        await DependencyService
                            .Get<IAlertService>()
                            .Inform("Пароль изменён");
                    }
                    else if (response.StatusCode == HttpStatusCode.Forbidden)
                    {
                        await DependencyService
                            .Get<IAlertService>()
                            .InformError(
                                JsonConvert.DeserializeObject<string>(
                                    await response.Content.ReadAsStringAsync()));
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

        public Task<bool> DeleteItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<ChangePasswordCredentials> GetItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ChangePasswordCredentials>> GetItemsAsync(bool forceRefresh = false)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateItemAsync(ChangePasswordCredentials item)
        {
            throw new NotImplementedException();
        }
    }
}
