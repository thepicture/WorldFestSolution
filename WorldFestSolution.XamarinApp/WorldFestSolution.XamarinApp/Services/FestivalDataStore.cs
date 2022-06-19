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
        /// <summary>
        /// Добавляет новый фестиваль или обновляет существующий при условии 
        /// отсутствия ошибок валидации.
        /// </summary>
        /// <param name="item">Сохраняемый фестиваль.</param>
        /// <returns><see langword="true"/>, если фестиваль сохранён, 
        /// в противном случае <see langword="false"/>.</returns>
        public async Task<bool> AddItemAsync(Festival item)
        {
            // Объявление списка ошибок.
            StringBuilder errors = new StringBuilder();

            // Если названия нет, то фестиваль невалидный.
            if (string.IsNullOrWhiteSpace(item.Title))
            {
                _ = errors.AppendLine("Введите название");
            }

            if (string.IsNullOrWhiteSpace(item.Address))
            {
                _ = errors.AppendLine("Введите адрес фестиваля");
            }

            // Если фестиваль начинается раньше сегодняшнего дня, то он невалидный.
            if (item.FromDateTime <= DateTime.Now)
            {
                _ = errors.AppendLine("Дата начала фестиваля " +
                    "должна быть позднее текущей даты");
            }

            // Если программ в фестивале нет, то он невалидный.
            if (item.FestivalProgram.Count == 0)
            {
                _ = errors.AppendLine("Создайте хотя бы одну " +
                    "программу для фестиваля");
            }

            // Если есть ошибки валидации, то покинуть метод.
            if (errors.Length > 0)
            {
                await DependencyService
                    .Get<IAlertService>()
                    .InformError(errors);
                return false;
            }

            // Сериализация фестиваля для отправки по API.
            string jsonFestival = JsonConvert.SerializeObject(item);

            // Объявление Http-клиента для связи с сервером.
            using (HttpClient client = await DependencyService.Get<IHttpContextFactory>().GetInstance())
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Basic",
                                                  Identity.AuthorizationValue);
                client.BaseAddress = new Uri(Api.BaseUrl);

                // Попытка сохранить фестиваль
                try
                {
                    HttpResponseMessage response = await client
                     .PostAsync("festivals",
                                new StringContent(jsonFestival,
                                                  Encoding.UTF8,
                                                  "application/json"));

                    // Если ответ от сервера "Получен", то фестиваль сохранён.
                    if (response.StatusCode == HttpStatusCode.Created)
                    {
                        await DependencyService
                            .Get<IAlertService>()
                            .Inform("Фестиваль сохранён");
                    }

                    // В противном случае вызов обратной связи с причиной ошибки.
                    else
                    {
                        Debug.WriteLine(response);
                        await DependencyService
                            .Get<IAlertService>()
                            .InformError(response);
                    }

                    // Метод возвращает результат значения "Сохранён ли фестиваль".
                    return response.StatusCode == HttpStatusCode.Created;
                }

                /* Показать пользователю причину ошибки, если она не распознана. 
                 * Этот сценарий может произойти, если сертификат сервера невалидный
                 * или превышен лимит обращений к серверу.
                */
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
            using (HttpClient client = await DependencyService.Get<IHttpContextFactory>().GetInstance())
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
            using (HttpClient client = await DependencyService.Get<IHttpContextFactory>().GetInstance())
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
            using (HttpClient client = await DependencyService.Get<IHttpContextFactory>().GetInstance())
            {
                client.DefaultRequestHeaders.Authorization =
                     new AuthenticationHeaderValue("Basic",
                                                   Identity.AuthorizationValue);
                client.BaseAddress = new Uri(Api.BaseUrl);
                try
                {
                    HttpResponseMessage response = await client
                        .GetAsync($"festivals?isRelatedToMe=false");
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
