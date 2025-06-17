using CinemaAppWPF.DTO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace CinemaAppWPF.Services
{
    public class HallService
    {
        private readonly HttpClient _httpClient;

        public HallService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5000/api/"),
                Timeout = TimeSpan.FromSeconds(30)
            };
        }

        public async Task<List<hallDto>> GetHallsAsync()
        {
            var response = await _httpClient.GetAsync("hall");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<hallDto>>() ?? new List<hallDto>();
        }

        public async Task<hallDto?> GetHallByIdAsync(Guid id)
        {
            var response = await _httpClient.GetAsync($"hall/{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<hallDto>();
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
            response.EnsureSuccessStatusCode();
            return null;
        }

        public async Task CreateHallAsync(hallDto halldto)
        {
            var response = await _httpClient.PostAsJsonAsync("hall", halldto);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateHallAsync(hallDto halldto)
        {
            var response = await _httpClient.PutAsJsonAsync("hall", halldto);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteHallAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"hall/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}