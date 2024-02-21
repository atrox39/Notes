using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Notes.Data.DTOs;
using System.Security.Claims;
using System.Text.Json;
using System.Net.Http.Headers;
using Notes.Front.Util;

namespace Notes.Front.Provider
{
  public class UserProvider : AuthenticationStateProvider
  {
    private readonly HttpClient http;
    private readonly IJSRuntime jsr;
    private readonly AuthenticationState anonymous;

    public UserProvider(HttpClient http, IJSRuntime jsr)
    {
      this.http = http;
      this.jsr = jsr;
      anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    }

    public async override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
      var dToken = await jsr.InvokeAsync<string>("localStorage.getItem", "jwt").ConfigureAwait(false) ?? string.Empty;
      if (string.IsNullOrEmpty(dToken))
      {
        return anonymous;
      }
      var token = JsonSerializer.Deserialize<TokenDto>(dToken);
      if (token is null)
      {
        return anonymous;
      }
      http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token.Token);
      var auth = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(token.Token), "JWT")));
      return auth;
    }

    public void NotifyUserAuthentication(string token)
    {
      var claims = JwtParser.ParseClaimsFromJwt(token);
      var user = new ClaimsIdentity(claims, "JWT");
      NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal(user))));
    }

    public void NotifyUserLogout()
    {
      NotifyAuthenticationStateChanged(Task.FromResult(anonymous));
    }
  }
}
