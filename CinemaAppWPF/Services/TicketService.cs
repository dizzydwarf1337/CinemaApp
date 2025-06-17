using CinemaAppWPF.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CinemaAppWPF.Services
{
    public class TicketService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseApiUrl = "http://localhost:5000/api/ticket";

        public TicketService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<Guid> CreateTicketAsync(ticketDto ticketDto)
        {
            var jsonContent = JsonSerializer.Serialize(ticketDto);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(_baseApiUrl, content);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Guid>(responseBody);
        }

        public async Task<ICollection<ticketDto>> GetAllTicketsAsync()
        {
            var response = await _httpClient.GetAsync(_baseApiUrl);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ICollection<ticketDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<ticketDto>();
        }

        public async Task<ICollection<ticketDto>> GetTicketsByUserIdAsync(Guid userId)
        {
            var response = await _httpClient.GetAsync($"{_baseApiUrl}/user/{userId}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ICollection<ticketDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<ticketDto>();
        }

        public async Task UpdateTicketAsync(ticketDto ticketDto)
        {
            var jsonContent = JsonSerializer.Serialize(ticketDto);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(_baseApiUrl, content);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteTicketAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"{_baseApiUrl}/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}