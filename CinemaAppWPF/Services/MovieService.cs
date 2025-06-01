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
    public class MovieService
    {
        private readonly HttpClient _httpClient;

        public MovieService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5000/api/")
            };
        }

        public async Task<List<MovieDto>> GetMoviesAsync()
        {
            var response = await _httpClient.GetAsync("movie");
            response.EnsureSuccessStatusCode();
            var movies = await response.Content.ReadFromJsonAsync<List<MovieDto>>();
            return movies ?? new List<MovieDto>();
        }

        public async Task<MovieDto?> GetMovieByIdAsync(Guid id)
        {
            var response = await _httpClient.GetAsync($"movie/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<MovieDto>();
        }

        public async Task CreateMovieAsync(MovieDto movieDto)
        {
            var response = await _httpClient.PostAsJsonAsync("movie", movieDto);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateMovieAsync(MovieDto movieDto)
        {
            var response = await _httpClient.PutAsJsonAsync("movie", movieDto);
            response.EnsureSuccessStatusCode();
        }

        public async Task UploadImageAsync(Guid movieId, MultipartFormDataContent formData)
        {
            var response = await _httpClient.PutAsync($"movie/{movieId}", formData);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteMovieByIdAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"movie/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
