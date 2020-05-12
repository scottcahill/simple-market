using System;
using global::SmWeb.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text;

namespace SmWeb.Services
{
    public class MemberDS : IMemberDS
    {

        private readonly HttpClient _httpClient;

        public MemberDS(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> AddMember(Member member)
        {
            var memberJson =
                new StringContent(JsonSerializer.Serialize(member), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/member", memberJson);

            //if (response.IsSuccessStatusCode)
            //{
            //    return await JsonSerializer.DeserializeAsync<Member>(await response.Content.ReadAsStreamAsync());
            //}

            return response.IsSuccessStatusCode;
        }

        public async Task DeleteMember(string memberId)
        {
            await _httpClient.DeleteAsync($"api/member/{memberId}");
        }

        public async Task<IEnumerable<Member>> GetAllMembers()
        {
            return await JsonSerializer.DeserializeAsync<IEnumerable<Member>>
                (await _httpClient.GetStreamAsync($"api/member"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task<Member> GetMemberById(string memberId)
        {
            return await JsonSerializer.DeserializeAsync<Member>
               (await _httpClient.GetStreamAsync($"api/member/{memberId}"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task UpdateMember(Member member)
        {
            var memberJson =
               new StringContent(JsonSerializer.Serialize(member), Encoding.UTF8, "application/json");

            await _httpClient.PutAsync("api/member", memberJson);
        }
    }
}

