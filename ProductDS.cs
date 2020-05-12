using System;
using global::SmWeb.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text;

namespace SmWeb.Services
{
    public class ProductDS : IProductDS
    {

        private readonly HttpClient _httpClient;

        public ProductDS(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> AddProduct(Product Product)
        {
            var ProductJson =
                new StringContent(JsonSerializer.Serialize(Product), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/Product", ProductJson);

            //if (response.IsSuccessStatusCode)
            //{
            //    return await JsonSerializer.DeserializeAsync<Product>(await response.Content.ReadAsStreamAsync());
            //}

            return response.IsSuccessStatusCode;
        }

        public async Task DeleteProduct(string ProductId)
        {
            await _httpClient.DeleteAsync($"api/Product/{ProductId}");
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await JsonSerializer.DeserializeAsync<IEnumerable<Product>>
                (await _httpClient.GetStreamAsync($"api/Product"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task<Product> GetProductById(string ProductId)
        {
            return await JsonSerializer.DeserializeAsync<Product>
               (await _httpClient.GetStreamAsync($"api/Product/{ProductId}"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task UpdateProduct(Product Product)
        {
            var ProductJson =
               new StringContent(JsonSerializer.Serialize(Product), Encoding.UTF8, "application/json");

            await _httpClient.PutAsync("api/Product", ProductJson);
        }
    }
}

