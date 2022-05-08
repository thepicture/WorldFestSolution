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
    class FestivalResponseInviteDataStore : IDataStore<IEnumerable<ResponseInvite>>
    {
        public Task<bool> AddItemAsync(IEnumerable<ResponseInvite> item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ResponseInvite>> GetItemAsync(string id)
        {
            using (HttpClient client = new HttpClient(App.ClientHandler))
            {
                client.DefaultRequestHeaders.Authorization =
                     new AuthenticationHeaderValue("Basic",
                                                   Identity.AuthorizationValue);
                client.BaseAddress = new Uri(Api.BaseUrl);
                try
                {
                    HttpResponseMessage response = await client
                        .GetAsync($"ParticipantInvites?festivalId={id}");
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert
                            .DeserializeObject<IEnumerable<ResponseInvite>>(
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
            return new List<ResponseInvite>();
        }

        public Task<IEnumerable<IEnumerable<ResponseInvite>>> GetItemsAsync(bool forceRefresh = false)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateItemAsync(IEnumerable<ResponseInvite> item)
        {
            throw new NotImplementedException();
        }
    }
}
