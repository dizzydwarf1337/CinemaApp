using CinemaAppWPF.DTO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

public class LoginService
{
    private readonly HttpClient _httpClient;

    public LoginService()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:5000/api/") 
        };
    }

    public async Task<LoginResponse?> LoginAsync(LoginRequest loginRequest)
    {
        var response = await _httpClient.PostAsJsonAsync("login", loginRequest);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<LoginResponse>();
        }
        return null; 
    }
}
