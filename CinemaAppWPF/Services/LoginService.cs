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
    public async Task<RegistrationResponse?> RegisterAsync(UserDto userRegistrationDto) 
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("user", userRegistrationDto); 

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<RegistrationResponse>();
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return new RegistrationResponse { Message = $"Registration failed: {response.ReasonPhrase}. {errorContent}" };
            }
        }
        catch (HttpRequestException httpEx)
        {
            return new RegistrationResponse { Message = $"Network error during registration: {httpEx.Message}" };
        }
        catch (Exception ex)
        {
            return new RegistrationResponse {  Message = $"An unexpected error occurred during registration: {ex.Message}" };
        }
    }
}

