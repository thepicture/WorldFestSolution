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
            string result = await DependencyService
                .Get<IAlertService>()
                .Prompt("Введите количество звёзд от 1 до 5",
                        maxLength: 1,
                        Keyboard.Numeric);
            if (string.IsNullOrWhiteSpace(result))
            {
                await DependencyService
                    .Get<IAlertService>()
                    .InformError("Вы ничего не ввели. "
                                 + "Оценка отменена");
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

            string jsonFestivalRating = JsonConvert.SerializeObject(item);
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Basic",
                                                  Identity.AuthorizationValue);
                client.BaseAddress = new Uri(Api.BaseUrl);
                try
                {
                    HttpResponseMessage response = await client
                     .PostAsync(new Uri(client.BaseAddress + "festivalratings"),
                                new StringContent(jsonFestivalRating,
                                                  Encoding.UTF8,
                                                  "application/json"));
                    string content = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode == HttpStatusCode.Created)
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            DependencyService
                                .Get<IAlertService>()
                                .Inform(
                                    JsonConvert.DeserializeObject<string>(content));
                        });
                        return true;
                    }
                    else if (response.StatusCode == HttpStatusCode.InternalServerError)
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            DependencyService
                                .Get<IAlertService>()
                                .InformError(
                                    "Произошла ошибка сервера. Подождите " +
                                    "и попробуйте ещё раз");
                        });
                    }
                    else
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            DependencyService
                               .Get<IAlertService>()
                               .InformError(
                                   $"Произошла ошибка {response.StatusCode}. " +
                                   $"Подождите и попробуйте ещё раз");
                        });
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
            return false;
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
