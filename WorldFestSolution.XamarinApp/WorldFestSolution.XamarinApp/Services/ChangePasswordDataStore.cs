﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WorldFestSolution.XamarinApp.Models;
using WorldFestSolution.XamarinApp.Models.Serialized;
using Xamarin.Forms;

namespace WorldFestSolution.XamarinApp.Services
{
    public class ChangePasswordDataStore : IDataStore<ChangePasswordCredentials>
    {
        public async Task<bool> AddItemAsync(ChangePasswordCredentials item)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(item.OldPassword))
            {
                _ = errors.AppendLine("Введите старый пароль");
            }
            if (string.IsNullOrWhiteSpace(item.NewPassword))
            {
                _ = errors.AppendLine("Введите новый пароль");
            }
            if (!string.IsNullOrWhiteSpace(item.OldPassword)
                && !string.IsNullOrWhiteSpace(item.NewPassword)
                && item.OldPassword == item.NewPassword)
            {
                _ = errors.AppendLine("Новый пароль " +
                    "должен отличаться от старого пароля");
            }
            if (errors.Length > 0)
            {
                await DependencyService
                    .Get<IAlertService>()
                    .InformError(
                        errors.ToString());
                return false;
            }
            string jsonPasswordCredentials = JsonConvert.SerializeObject(item);
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(Api.BaseUrl);
                try
                {
                    HttpResponseMessage response = await client
                     .PostAsync(new Uri(client.BaseAddress + "users/changepassword"),
                                new StringContent(jsonPasswordCredentials,
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
                                    "Ошибка сервера: "
                                    + content);
                        });
                    }
                    else
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            DependencyService
                                .Get<IAlertService>()
                                .InformError(
                                    $"Произошла ошибка с кодом {response.StatusCode}");
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

        public Task<ChangePasswordCredentials> GetItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ChangePasswordCredentials>> GetItemsAsync(bool forceRefresh = false)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateItemAsync(ChangePasswordCredentials item)
        {
            throw new NotImplementedException();
        }
    }
}
