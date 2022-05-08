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
    public class FestivalRatingDataStore : IDataStore<FestivalRating>
    {
        public async Task<bool> AddItemAsync(FestivalRating item)
        {
            if (item.IsRated)
            {
                await DependencyService
                    .Get<IAlertService>()
                    .InformError("Вы уже оценили этот фестиваль");
                return false;
            }
            if (item.CountOfStars == 0)
            {
                string result = await DependencyService
                    .Get<IAlertService>()
                    .Prompt("Введите количество звёзд от 1 до 5",
                            maxLength: 1,
                            Keyboard.Numeric);
                if (string.IsNullOrWhiteSpace(result))
                {
                    await DependencyService
                        .Get<IAlertService>()
                        .InformError("Вы не указали оценку");
                    return false;
                }
                if (!int.TryParse(result, out int starsCount)
                    || starsCount < 1
                    || starsCount > 5)
                {
                    await DependencyService
                        .Get<IAlertService>()
                        .InformError("Количество звёзд - "
                                     + "это положительное число от 1 до 5");
                    return await AddItemAsync(item);
                }
                item.CountOfStars = starsCount;
            }

            string jsonFestivalRating = JsonConvert.SerializeObject(item);
            using (HttpClient client = new HttpClient(App.ClientHandler))
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Basic",
                                                  Identity.AuthorizationValue);
                client.BaseAddress = new Uri(Api.BaseUrl);
                try
                {
                    HttpResponseMessage response = await client
                     .PostAsync("festivalratings",
                                new StringContent(jsonFestivalRating,
                                                  Encoding.UTF8,
                                                  "application/json"));
                    if (response.StatusCode == HttpStatusCode.Created)
                    {
                        await DependencyService
                            .Get<IAlertService>()
                            .Inform("Оценка сохранена");
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

        public Task<FestivalRating> GetItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<FestivalRating>> GetItemsAsync(bool forceRefresh = false)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateItemAsync(FestivalRating item)
        {
            throw new NotImplementedException();
        }
    }
}
