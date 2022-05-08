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
using Xamarin.Forms;

namespace WorldFestSolution.XamarinApp.Services
{
    public class UserImageDataStore : IDataStore<byte[]>
    {
        public async Task<bool> AddItemAsync(byte[] item)
        {
            string imageBytes = JsonConvert.SerializeObject(item);
            using (HttpClient client = new HttpClient(App.ClientHandler))
            {
                client.DefaultRequestHeaders.Authorization =
                     new AuthenticationHeaderValue("Basic",
                                                   Identity.AuthorizationValue);
                client.BaseAddress = new Uri(Api.BaseUrl);
                try
                {
                    HttpResponseMessage response = await client
                        .PostAsync($"users/image", new StringContent(imageBytes,
                                                                     Encoding.UTF8,
                                                                     "application/json"));
                    if (response.StatusCode == HttpStatusCode.NoContent)
                    {
                        await DependencyService
                            .Get<IAlertService>()
                            .Inform("Фото изменено");
                    }
                    else
                    {
                        Debug.WriteLine(response);
                        await DependencyService
                            .Get<IAlertService>()
                            .InformError(response);
                    }
                    return response.StatusCode == HttpStatusCode.NoContent;
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

        public async Task<byte[]> GetItemAsync(string id)
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
                        .GetAsync($"users/image");
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert
                            .DeserializeObject<byte[]>(
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
            return null;
        }

        public Task<IEnumerable<byte[]>> GetItemsAsync(bool forceRefresh = false)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateItemAsync(byte[] item)
        {
            throw new NotImplementedException();
        }
    }
}
