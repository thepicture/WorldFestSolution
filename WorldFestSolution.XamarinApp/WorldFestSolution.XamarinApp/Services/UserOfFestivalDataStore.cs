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
    public class UserOfFestivalDataStore : IDataStore<UserOfFestival>
    {
        public Task<bool> AddItemAsync(UserOfFestival item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<UserOfFestival> GetItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserOfFestival>> GetItemsAsync(bool forceRefresh = false)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateItemAsync(UserOfFestival item)
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
                        .GetAsync($"festivals/toggleparticipatestate"
                        + $"?festivalId={item.FestivalId}");
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        string action = item.IsParticipating
                            ? "покинули"
                            : "вступили в";
                        await DependencyService
                            .Get<IAlertService>()
                            .Inform($"Вы {action} фестиваль");
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
