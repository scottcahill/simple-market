using System;
using global::SmWeb.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text;

namespace SmWeb.Services
{
    public class StoreDS : IStoreDS
    {

        private readonly HttpClient _httpClient;

        public StoreDS(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> AddStore(Store Store)
        {
            var StoreJson =
                new StringContent(JsonSerializer.Serialize(Store), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/Store", StoreJson);

            return response.IsSuccessStatusCode;
        }

        public async Task DeleteStore(string StoreId)
        {
            await _httpClient.DeleteAsync($"api/Store/{StoreId}");
        }

        public async Task<IEnumerable<Store>> GetAllStores()
        {
            return await JsonSerializer.DeserializeAsync<IEnumerable<Store>>
                (await _httpClient.GetStreamAsync($"api/Store"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task<Store> GetStoreById(string StoreId)
        {
            return await JsonSerializer.DeserializeAsync<Store>
               (await _httpClient.GetStreamAsync($"api/Store/{StoreId}"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task UpdateStore(Store Store)
        {
            var StoreJson =
               new StringContent(JsonSerializer.Serialize(Store), Encoding.UTF8, "application/json");

            await _httpClient.PutAsync("api/Store", StoreJson);
        }
    }
}

