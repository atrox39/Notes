using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Notes.Data.DTOs;
using Notes.Front.Provider;
using System.Net.Http.Json;
using System.Text.Json;
using System.Net.Http.Headers;

namespace Notes.Front.Service
{
  public interface IAuthService
  {
    public Task<TokenDto?> Login(LoginDto loginDto);
    public Task Logout();
  }
  public class AuthService(
    HttpClient http,
    AuthenticationStateProvider authProvider,
    IJSRuntime jsr
  ) : IAuthService
  { 
    private readonly string baseURL = "/api/auth";
    public async Task<TokenDto?> Login(LoginDto loginDto)
    {
      var result = await http.PostAsJsonAsync($"{baseURL}/login", loginDto);
      if (result.IsSuccessStatusCode)
      {
        var jwtRes = await result.Content.ReadFromJsonAsync<TokenDto>();
        if (jwtRes is null)
        {
          return null;
        }
        await jsr.InvokeVoidAsync("localStorage.setItem", "jwt", JsonSerializer.Serialize(jwtRes)).ConfigureAwait(false);
        ((UserProvider)authProvider).NotifyUserAuthentication(jwtRes.Token);
        http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", jwtRes.Token);
        return jwtRes;
      }
      else
      {
        return null;
      }
    }

    public async Task Logout()
    {
      await jsr.InvokeVoidAsync("localStorage.removeItem", "jwt").ConfigureAwait(false);
      ((UserProvider)authProvider).NotifyUserLogout();
      http.DefaultRequestHeaders.Authorization = null;
    }
  }
}
