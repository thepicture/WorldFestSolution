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
    public class RegistrationUserDataStore : IDataStore<RegistrationUser>
    {
        public async Task<bool> AddItemAsync(RegistrationUser item)
        {
            StringBuilder validationErrors = new StringBuilder();
            if (item.UserTypeId == 0)
            {
                _ = validationErrors.AppendLine("Укажите роль");
            }
            if (!item.Is18OrMoreYearsOld.HasValue)
            {
                _ = validationErrors.AppendLine("Укажите ваш возраст");
            }
            if (string.IsNullOrWhiteSpace(item.Login))
            {
                _ = validationErrors.AppendLine("Введите логин");
            }
            if (string.IsNullOrWhiteSpace(item.Password))
            {
                _ = validationErrors.AppendLine("Введите пароль");
            }
            if (string.IsNullOrWhiteSpace(item.LastName))
            {
                _ = validationErrors.AppendLine("Введите фамилию");
            }
            if (string.IsNullOrWhiteSpace(item.FirstName))
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

            string jsonUser = JsonConvert.SerializeObject(item);
            using (HttpClient client = new HttpClient(App.ClientHandler))
            {
                client.BaseAddress = new Uri(Api.BaseUrl);
                try
                {
                    StringContent content = new StringContent(jsonUser,
                                                              Encoding.UTF8,
                                                              App.Json);
                    HttpResponseMessage response = await client
                     .PostAsync("users/register",
                                content);
                    if (response.StatusCode == HttpStatusCode.Created)
                    {
                        await DependencyService
                            .Get<IAlertService>()
                            .Inform("Вы зарегистрированы");
                    }
                    else
                    {
                        await DependencyService
                            .Get<IAlertService>()
                            .InformError(response);
                    }
                    return response.StatusCode == HttpStatusCode.Created;
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

        public Task<RegistrationUser> GetItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<RegistrationUser>> GetItemsAsync(bool forceRefresh = false)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateItemAsync(RegistrationUser item)
        {
            throw new NotImplementedException();
        }
    }
}
