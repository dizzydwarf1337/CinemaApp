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
    }
}
