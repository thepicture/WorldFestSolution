﻿using Newtonsoft.Json;
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
                    .InformError(
                        errors.ToString());
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
                     .PostAsync(new Uri(client.BaseAddress + "festivals"),
                                new StringContent(jsonFestival,
                                                  Encoding.UTF8,
                                                  "application/json"));
                    string content = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode == HttpStatusCode.Created)
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            DependencyService
                                .Get<IAlertService>()
                                .Inform("Фестиваль сохранён");
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
                                    "Произошла внутренняя ошибка " +
                                    "сервера. Подождите минуту " +
                                    "и попробуйте добавить фестиваль " +
                                    "ещё раз");
                        });
                    }
                    else
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            DependencyService
                                .Get<IAlertService>()
                                .InformError($"Ошибка добавления (код {response.StatusCode}): "
                                    + content);
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
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            DependencyService.Get<IAlertService>()
                                .Inform($"Фестиваль удалён. "
                                + "Ваш рейтинг уменьшился");
                        });
                        return true;
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
                        string content = await response.Content
                            .ReadAsStringAsync();
                        return JsonConvert
                            .DeserializeObject<Festival>(content);
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
                        .Title == "Мои фестивали"
                        || AppShell
                        .Current
                        .CurrentItem
                        .CurrentItem
                        .Title == "Популярность фестов";
                    HttpResponseMessage response = await client
                        .GetAsync($"festivals?isRelatedToMe={isRelatedToMe}");
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        string content = await response.Content
                            .ReadAsStringAsync();
                        return JsonConvert
                            .DeserializeObject<IEnumerable<Festival>>(content);
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

        public Task<bool> UpdateItemAsync(Festival item)
        {
            throw new NotImplementedException();
        }
    }
}
