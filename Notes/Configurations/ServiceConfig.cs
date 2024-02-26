using System.Text;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
using Notes.Repository;
using Notes.Validations;




namespace Notes.Configurations
{
  public static class ServiceConfig
  {
    public static WebApplicationBuilder ServiceAppConfig(this WebApplicationBuilder builder)
    {
      builder.GlobalValidator(); // For Global validations
      builder.Services.AddDbContext<NotesContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));
      builder.Services.AddAuthorization();
      //builder.Services.UseAntiForgery()//To allow works with metadata in files request
            builder.Services.AddAuthentication("Bearer").AddJwtBearer(opt =>
      {
        var singning = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTSecret"]!));
        var credentials = new SigningCredentials(singning, SecurityAlgorithms.HmacSha256Signature);
        opt.RequireHttpsMetadata = false;
        opt.TokenValidationParameters = new TokenValidationParameters
        {
          ValidateAudience = false,
          ValidateIssuer = false,
          IssuerSigningKey = singning,
        };
      });
      builder.Services.AddOutputCache();
      builder.Services.AddCors();
      builder.Services.Configure<FormOptions>(options =>
      {
         options.MultipartBodyLengthLimit = 104857600;
      });
      //builder.Services.AddControllers(options =>
      //{
      //    options.ModelBinderProviders.Insert(0, new FormDataModelBinderProvider());
      //});

            builder.Services.AddEndpointsApiExplorer();      
      builder.Services.AddAntiforgery(); //Registrar el servicio de antiforjería en el método ConfigureServices():      
      builder.Services.AddSwaggerGen();      
      builder.Services.AddAutoMapper(typeof(Program));
      builder.Services.AddScoped<IAuthRepository, AuthRepository>();
      builder.Services.AddScoped<IJwtRepository, JwtRepository>();
      builder.Services.AddScoped<INoteRepository, NoteRepository>();
      builder.Services.AddScoped<IFileRepository,FileRepository>();
            return builder;
    }
  }
}