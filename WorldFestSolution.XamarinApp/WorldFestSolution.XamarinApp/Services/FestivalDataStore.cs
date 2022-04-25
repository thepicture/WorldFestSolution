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
    public class FestivalDataStore : IDataStore<Festival>
    {
        public async Task<bool> AddItemAsync(Festival item)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(item.Title))
            {
                _ = errors.AppendLine("Введите название");
            }
            if (item.FromDateTime <= DateTime.Now)
            {
                _ = errors.AppendLine("Дата начала фестиваля " +
                    "должна быть позднее текущей даты");
            }
            if (item.FestivalProgram.Count == 0)
            {
                _ = errors.AppendLine("Создайте хотя бы одну " +
                    "программу для фестиваля");
            }
            if (errors.Length > 0)
            {
                await DependencyService
                    .Get<IAlertService>()
                    .InformError(errors);
                return false;
            }
            string jsonFestival = JsonConvert.SerializeObject(item);
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Basic",
                                                  Identity.AuthorizationValue);
                client.BaseAddress = new Uri(Api.BaseUrl);
                try
                {
                    HttpResponseMessage response = await client
                     .PostAsync("festivals",
                                new StringContent(jsonFestival,
                                                  Encoding.UTF8,
                                                  "application/json"));
                    if (response.StatusCode == HttpStatusCode.Created)
                    {
                        await DependencyService
                            .Get<IAlertService>()
                            .Inform("Фестиваль сохранён");
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

        public async Task<bool> DeleteItemAsync(string id)
        {
            bool isUserWantsToDelete = await DependencyService
                .Get<IAlertService>()
                .Ask("Удалить фестиваль? "
                     + "При удалении фестиваля "
                     + "у вас уменьшится рейтинг");
            if (!isUserWantsToDelete)
            {
                return false;
            }
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization =
                     new AuthenticationHeaderValue("Basic",
                                                   Identity.AuthorizationValue);
                client.BaseAddress = new Uri(Api.BaseUrl);
                try
                {
                    HttpResponseMessage response = await client
                        .DeleteAsync($"festivals/{id}");
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        await DependencyService.Get<IAlertService>()
                            .Inform($"Фестиваль удалён. "
                            + "Ваш рейтинг уменьшился");
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

        public async Task<Festival> GetItemAsync(string id)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization =
                     new AuthenticationHeaderValue("Basic",
                                                   Identity.AuthorizationValue);
                client.BaseAddress = new Uri(Api.BaseUrl);
                try
                {
                    HttpResponseMessage response = await client
                        .GetAsync($"festivals/{id}");
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert
                            .DeserializeObject<Festival>(
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

        public async Task<IEnumerable<Festival>> GetItemsAsync(bool forceRefresh = false)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization =
                     new AuthenticationHeaderValue("Basic",
                                                   Identity.AuthorizationValue);
                client.BaseAddress = new Uri(Api.BaseUrl);
                try
                {
                    bool isRelatedToMe = AppShell
                        .Current
                        .CurrentItem
                        .CurrentItem
                        .Title == "Мои фестивали";
                    HttpResponseMessage response = await client
                        .GetAsync($"festivals?isRelatedToMe={isRelatedToMe}");
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert
                            .DeserializeObject<IEnumerable<Festival>>(
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
            return new List<Festival>();
        }

        public Task<bool> UpdateItemAsync(Festival item)
        {
            throw new NotImplementedException();
        }
    }
}
