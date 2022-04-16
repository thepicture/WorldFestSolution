using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WorldFestSolution.XamarinApp.Models;
using WorldFestSolution.XamarinApp.Models.Serialized;
using WorldFestSolution.XamarinApp.ViewModels;
using Xamarin.Forms;

namespace WorldFestSolution.XamarinApp.Services
{
    public class InviteDataStore : IDataStore<Invite>
    {
        public Task<bool> AddItemAsync(Invite item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Invite> GetItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Invite>> GetItemsAsync(
            bool forceRefresh = false)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization =
                     new AuthenticationHeaderValue("Basic",
                                                   Identity.AuthorizationValue);
                client.BaseAddress = new Uri(Api.BaseUrl);
                int festivalId = (AppShell.Current
                    .Navigation.NavigationStack.Last().BindingContext
                    as InviteViewModel).FestivalId;
                try
                {
                    HttpResponseMessage response = await client
                        .GetAsync("ParticipantInvites?"
                        + $"festivalId={festivalId}");
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        string content = await response.Content
                            .ReadAsStringAsync();
                        return JsonConvert
                            .DeserializeObject<IEnumerable<Invite>>(content);
                    }
                }
                catch (HttpRequestException ex)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        DependencyService.Get<IAlertService>()
                            .InformError("Ошибка запроса: " + ex.StackTrace);
                    });
                    Debug.WriteLine(ex.StackTrace);
                }
                catch (TaskCanceledException ex)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        DependencyService.Get<IAlertService>()
                            .InformError("Запрос отменён: " + ex.StackTrace);
                    });
                    Debug.WriteLine(ex.StackTrace);
                }
                catch (InvalidOperationException ex)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        DependencyService.Get<IAlertService>()
                            .InformError("Операция некорректна: " + ex.StackTrace);
                    });
                    Debug.WriteLine(ex.StackTrace);
                }
                catch (Exception ex)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        DependencyService.Get<IAlertService>()
                            .InformError("Неизвестная ошибка: " + ex.StackTrace);
                    });
                    Debug.WriteLine(ex.StackTrace);
                }
            }
            return null;
        }

        public Task<bool> UpdateItemAsync(Invite item)
        {
            throw new NotImplementedException();
        }
    }
}
