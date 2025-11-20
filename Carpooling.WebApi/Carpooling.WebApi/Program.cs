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
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// ------------------ MONGO REPOS ------------------
builder.Services.AddScoped<IRepository<User>>(sp => new MongoRepository<User>("User"));
builder.Services.AddScoped<IRepository<Vehicle>>(sp => new MongoRepository<Vehicle>("Vehicle"));
builder.Services.AddScoped<IRepository<Ride>>(sp => new MongoRepository<Ride>("Ride"));
builder.Services.AddScoped<IRepository<Booking>>(sp => new MongoRepository<Booking>("Booking"));

// ------------------ SERVICES ------------------
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IVehicleService, VehicleService>();
builder.Services.AddScoped<IRideService, RideService>();
builder.Services.AddScoped<IBookingService, BookingService>();

builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddSingleton<JwtTokenGenerator>();

// ------------------ PORT FIX FOR RAILWAY ------------------
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(int.Parse(port));
});

// ------------------ JWT AUTH ------------------
var jwtSettings = builder.Configuration.GetSection("JwtSettings");

builder.Services.AddAuthentication(o =>
{
    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]))
    };
});

// ------------------ SWAGGER ------------------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ------------------ VALIDATION ------------------
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<VehicleValidator>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// ------------------ ROUTING ------------------
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// ------------------ RUN ------------------
app.Run();
