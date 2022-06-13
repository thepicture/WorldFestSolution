using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WorldFestSolution.XamarinApp.Models;
using Xamarin.Forms;

namespace WorldFestSolution.XamarinApp.Services
{
    public class CountOfMyInvitesDataStore : IDataStore<int>
    {
        public Task<bool> AddItemAsync(int item)
        {
            throw new NotImplementedException();
        }
        public Task<bool> DeleteItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<int> GetItemAsync(string id)
        {
            using (HttpClient client = await DependencyService.Get<IHttpContextFactory>().GetInstance())
            {
                client.DefaultRequestHeaders.Authorization =
                     new AuthenticationHeaderValue("Basic",
                                                   Identity.AuthorizationValue);
                client.BaseAddress = new Uri(Api.BaseUrl);
                try
                {
                    HttpResponseMessage response = await client
                        .GetAsync("users/myInvites");
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert
                            .DeserializeObject<int>(
                                await response.Content.ReadAsStringAsync());
                    }
                    else
                    {
                        await DependencyService
                            .Get<IAlertService>()
                            .InformError(response);
                    }
                }
                catch (Exception ex)
                {
                    await DependencyService
                        .Get<IAlertService>()
                        .InformError(ex);
                }
            }
            return default;
        }

        public Task<IEnumerable<int>> GetItemsAsync(bool forceRefresh = false)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateItemAsync(int item)
        {
            throw new NotImplementedException();
        }
    }
}
