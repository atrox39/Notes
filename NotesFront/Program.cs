using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Notes.Front;
using Notes.Front.Service;
using Microsoft.AspNetCore.Components.Authorization;
using Notes.Front.Provider;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

string BASE_URL = Environment.GetEnvironmentVariable("API_URL") ?? "http://localhost:5012";

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(BASE_URL) });
builder.Services.AddScoped<UserProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(s => s.GetRequiredService<UserProvider>());
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<INoteService, NoteService>();

await builder.Build().RunAsync();
