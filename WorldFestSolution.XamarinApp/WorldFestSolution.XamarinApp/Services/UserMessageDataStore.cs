using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WorldFestSolution.XamarinApp.Models;
using WorldFestSolution.XamarinApp.Models.Serialized;
using Xamarin.Forms;

namespace WorldFestSolution.XamarinApp.Services
{
    public class UserMessageDataStore : IDataStore<UserMessage>
    {
        public async Task<bool> AddItemAsync(UserMessage item)
        {
            if (string.IsNullOrWhiteSpace(item.Message))
            {
                await DependencyService
                    .Get<IAlertService>()
                    .InformError("Введите сообщение");
                return false;
            }
            string jsonMessage = JsonConvert.SerializeObject(item);
            using (HttpClient client = await DependencyService.Get<IHttpContextFactory>().GetInstance())
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Basic",
                                                  Identity.AuthorizationValue);
                client.BaseAddress = new Uri(Api.BaseUrl);
                try
                {
                    HttpResponseMessage response = await client
                     .PostAsync("UserMessages",
                                new StringContent(jsonMessage,
                                                  Encoding.UTF8,
                                                  "application/json"));
                    if (response.StatusCode == HttpStatusCode.Created)
                    {
                        await DependencyService
                            .Get<IAlertService>()
                            .Inform("Сообщение отправлено");
                        return true;
                    }
                    else if (response.StatusCode == HttpStatusCode.Forbidden)
                    {
                        await DependencyService
                            .Get<IAlertService>()
                            .InformError(JsonConvert.DeserializeObject<string>(
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

        public Task<UserMessage> GetItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserMessage>> GetItemsAsync(bool forceRefresh = false)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateItemAsync(UserMessage item)
        {
            throw new NotImplementedException();
        }
    }
}
