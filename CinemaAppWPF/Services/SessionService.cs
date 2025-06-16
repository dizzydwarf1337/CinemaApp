using CinemaAppWPF.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace CinemaAppWPF.Services
{
    public class SessionService
    {
        private readonly HttpClient _httpClient;

        public SessionService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5000/api/")
            };
        }

        public async Task<List<sessionDto>> GetSessionsAsync()
        {
            var response = await _httpClient.GetAsync("session");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<sessionDto>>() ?? new();
        }

        public async Task CreateSessionAsync(sessionDto session)
        {
            var response = await _httpClient.PostAsJsonAsync("session", session);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateSessionAsync(sessionDto session)
        {
            var response = await _httpClient.PutAsJsonAsync("session", session);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteSessionAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"session/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
