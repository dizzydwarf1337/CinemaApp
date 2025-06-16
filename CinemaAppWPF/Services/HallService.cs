using CinemaAppWPF.DTO;
using System.Net.Http;
using System.Net.Http.Json;


namespace CinemaAppWPF.Services
{
    public class HallService
    {
        private readonly HttpClient _httpClient;

        public HallService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5000/api/") 
            };
        }

        public async Task<List<hallDto>> GetHallsAsync()
        {
            var response = await _httpClient.GetAsync("hall");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<hallDto>>() ?? new List<hallDto>();
        }
    }
}
