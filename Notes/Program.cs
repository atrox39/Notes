using Notes.Routes;
using Notes.Configurations;
using Notes.Data.Models;

var builder = WebApplication.CreateBuilder(args);
builder.ServiceAppConfig();

var app = builder.Build();

app.UseAntiforgery(); //Usar el servicio de antiforjería en la pipeline de solicitudes
app.MiddlewareAppConfig();

app.MapGet("/", () => Results.Redirect("/swagger"));
app.MapGet("/private", () => Results.Ok("This url is privated only view if you have a token")).RequireAuthorization();


app.MapGroup("/api").MapApi();

app.Run();
