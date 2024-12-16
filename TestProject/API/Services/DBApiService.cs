using System.Text;
using System.Text.Json;
using API.DTOS;
using API.Interfaces;

namespace API.Services;

public class DBApiService : IDBApiService
{
    private readonly HttpClient _httpClient;    
    public DBApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    } 
    public async Task<string> Login(LoginDTO loginDTO)
    {
        var loginDtoContent = new StringContent(
        System.Text.Json.JsonSerializer.Serialize(loginDTO), 
        Encoding.UTF8, 
        "application/json");     

        var response = await _httpClient.PostAsync("api/account/login", loginDtoContent);
        var content = await response.Content.ReadAsStringAsync();
        return content;    
    }

    public async Task<string> Register(LoginDTO loginDTO)
    {
        var loginDtoContent = new StringContent(
        System.Text.Json.JsonSerializer.Serialize(loginDTO), 
        Encoding.UTF8, 
        "application/json");     

        var response = await _httpClient.PostAsync("api/account/register", loginDtoContent);
        var content = await response.Content.ReadAsStringAsync();
        return content;    
    }
}
