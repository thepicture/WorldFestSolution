using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WorldFestSolution.XamarinApp.Models;
using WorldFestSolution.XamarinApp.Models.Serialized;
using Xamarin.Forms;

namespace WorldFestSolution.XamarinApp.Services
{
    public class UserMessagesDataStore : IDataStore<IEnumerable<UserMessage>>
    {
        public Task<bool> AddItemAsync(IEnumerable<UserMessage> item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<UserMessage>> GetItemAsync(string id)
        {
            using (HttpClient client = await DependencyService.Get<IHttpContextFactory>().GetInstance())
            {
                client.DefaultRequestHeaders.Authorization =
                     new AuthenticationHeaderValue("Basic", Identity.AuthorizationValue);
                client.BaseAddress = new Uri(Api.BaseUrl);
                try
                {
                    HttpResponseMessage response = await client
                        .GetAsync($"UserMessages?receiverId={id}");
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert
                            .DeserializeObject<IEnumerable<UserMessage>>(
                                await response.Content.ReadAsStringAsync());
                    }
                    else
                    {
                        Debug.WriteLine(response);
                        await DependencyService
                            .Get<IAlertService>()
                            .InformError(response);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    await DependencyService
                        .Get<IAlertService>()
                        .InformError(ex);
                }
            }
            return new List<UserMessage>();
        }

        public Task<IEnumerable<IEnumerable<UserMessage>>> GetItemsAsync(bool forceRefresh = false)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateItemAsync(IEnumerable<UserMessage> item)
        {
            throw new NotImplementedException();
        }
    }
}
