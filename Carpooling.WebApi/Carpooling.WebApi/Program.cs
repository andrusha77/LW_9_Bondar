using System.Text;
using Carpooling.WebApi.Controllers;
using Carpooling.WebApi.Interfaces;
using Carpooling.WebApi.Models;
using Carpooling.WebApi.Repositories;
using Carpooling.WebApi.Services;
using Carpooling.WebApi.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
var builder = WebApplication.CreateBuilder(args);
//4 task
// MongoDB configuration
var mongoUri = Environment.GetEnvironmentVariable("MONGO_URI")
               ?? "mongodb://localhost:27017"; 

builder.Services.AddScoped<IRepository<User>>(sp => new MongoRepository<User>(mongoUri, "User"));
builder.Services.AddScoped<IRepository<Vehicle>>(sp => new MongoRepository<Vehicle>(mongoUri, "Vehicle"));
builder.Services.AddScoped<IRepository<Ride>>(sp => new MongoRepository<Ride>(mongoUri, "Ride"));
builder.Services.AddScoped<IRepository<Booking>>(sp => new MongoRepository<Booking>(mongoUri, "Booking"));


var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(int.Parse(port));
});

// Services registration
builder.Services.AddScoped<Carpooling.WebApi.Interfaces.IUserService, Carpooling.WebApi.Services.UserService>();
builder.Services.AddScoped<Carpooling.WebApi.Interfaces.IVehicleService, Carpooling.WebApi.Services.VehicleService>();
builder.Services.AddScoped<Carpooling.WebApi.Interfaces.IRideService, Carpooling.WebApi.Services.RideService>();
builder.Services.AddScoped<Carpooling.WebApi.Interfaces.IBookingService, Carpooling.WebApi.Services.BookingService>();


//6 task
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IUserService, UserService>();


// Реєстрація PasswordHasher у DI
builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();



// Реєстрація JwtSettings та JwtTokenGenerator у DI
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddSingleton<JwtTokenGenerator>();


var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]))
    };
});

// Swagger configuration
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "My API", Version = "v1" });

    // Додаємо JWT авторизацію
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Введіть токен у форматі: Bearer {токен}"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<VehicleValidator>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    // Той самий шлях, який ти щойно відкривав у браузері і який точно працює:
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Carpooling.WebApi v1");
    c.RoutePrefix = "swagger"; // => UI на http://localhost:5097/swagger
});

app.MapControllers();
app.MapGet("/", ()=>"working \n gooooood");

app.UseAuthentication();
app.UseAuthorization();

app.Run();

