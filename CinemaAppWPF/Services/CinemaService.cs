using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.IO;
using CinemaAppWPF.DTO;
using System.Windows;

public class CinemaService
{
    private readonly HttpClient _httpClient;

    public CinemaService()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:5000/api/") 
        };
    }

    public async Task<ObservableCollection<CinemaDto>> GetCinemasAsync()
    {
        var cinemas = await _httpClient.GetFromJsonAsync<List<CinemaDto>>("cinema");
        return new ObservableCollection<CinemaDto>(cinemas);
    }

    public async Task<CinemaDto> GetCinemaByIdAsync(Guid id)
    {
        return await _httpClient.GetFromJsonAsync<CinemaDto>($"cinema/{id}");
    }

    public async Task CreateCinemaAsync(CreateCinemaRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("cinema", request);
        var responseBody = await response.Content.ReadAsStringAsync();

        MessageBox.Show("Response status: " + response.StatusCode + "\n" + responseBody + "\n" + response.Content);

    }



    public async Task UpdateCinemaAsync(CinemaDto cinemaDto)
    {
        var response = await _httpClient.PutAsJsonAsync("Cinema", cinemaDto);
        var responseBody = await response.Content.ReadAsStringAsync();

        MessageBox.Show("Response status: " + response.StatusCode + "\n" + responseBody);
    }

    public async Task DeleteCinemaAsync(Guid id)
    {
        await _httpClient.DeleteAsync($"Cinema/{id}");
    }
}
