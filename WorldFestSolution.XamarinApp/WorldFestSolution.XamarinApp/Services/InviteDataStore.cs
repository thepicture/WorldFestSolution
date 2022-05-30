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
    public class InviteDataStore : IDataStore<ResponseInvite>
    {
        public async Task<bool> AddItemAsync(ResponseInvite item)
        {
            if (item.ParticipantId != Identity.Id && !item.IsParticipantWantsInvites)
            {
                await DependencyService
                            .Get<IAlertService>()
                            .InformError("Участник выбрал опцию не получать приглашения");
                return false;
            }
            RequestInvite requestInvite = new RequestInvite
            {
                ParticipantId = item.ParticipantId,
                FestivalId = item.FestivalId
            };
            if (item.Organizer != null)
            {
                requestInvite.IsAccepted = item.IsAccepted;
                requestInvite.Id = item.Id;
            }
            string jsonRequestInvite = JsonConvert.SerializeObject(requestInvite);
            using (HttpClient client = new HttpClient(App.ClientHandler))
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Basic",
                                                  Identity.AuthorizationValue);
                client.BaseAddress = new Uri(Api.BaseUrl);
                try
                {
                    HttpResponseMessage response = await client
                     .PostAsync("participantInvites",
                                new StringContent(jsonRequestInvite,
                                                  Encoding.UTF8,
                                                  "application/json"));
                    string content = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode == HttpStatusCode.Created)
                    {
                        await DependencyService
                            .Get<IAlertService>()
                            .Inform("Приглашение обработано");
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

        public Task<ResponseInvite> GetItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ResponseInvite>> GetItemsAsync(
            bool forceRefresh = false)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateItemAsync(ResponseInvite item)
        {
            throw new NotImplementedException();
        }
    }
}
