using BusinessLayer;
using BusinessLayer.Interface;
using BusinessLayer.Service;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Context;
using RepositoryLayer.Interface;
using RepositoryLayer.Service;
using System;
using FluentValidation;
using FluentValidation.AspNetCore;
using BusinessLayer.Validators;
using ModelLayer.Model;
using AutoMapper;
using RepositoryLayer.Context;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ModelLayer.Config;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using StackExchange.Redis;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register DbContext
var ConnectionString = builder.Configuration.GetConnectionString("SqlConnection");
builder.Services.AddDbContext<AddressBookContext>(option => option.UseSqlServer(ConnectionString));


builder.Services.AddScoped<IAddressBookBL, AddressBookBL>();
builder.Services.AddScoped<IAddressBookRL, AddressBookRL>();
builder.Services.AddAutoMapper(typeof(AddressBookProfile));
builder.Services.AddControllers().AddFluentValidation();
builder.Services.AddControllers()
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<AddressBookValidator>());
builder.Services.AddScoped<IAddressBookService, AddressBookService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));
builder.Services.AddSingleton<IConfiguration>(builder.Configuration); 
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));
builder.Services.AddAutoMapper(typeof(AddressBookProfile));
builder.Services.AddScoped<RedisCacheService>();

var redisConfig = builder.Configuration.GetSection("Redis")["ConnectionString"];
if (string.IsNullOrEmpty(redisConfig))
{
    throw new ArgumentNullException("Redis ConnectionString is missing in appsettings.json");
}
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = redisConfig;
    options.InstanceName = builder.Configuration.GetSection("Redis")["InstanceName"];
    options.Configuration = builder.Configuration.GetSection("Redis")["ConnectionString"];
});
  
builder.Services.AddSingleton<RedisCacheService>();
builder.Services.AddSingleton<RabbitMQService>();





var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.UTF8.GetBytes(jwtSettings["Secret"]);
string secretKey = jwtSettings["Secret"];

if (string.IsNullOrEmpty(secretKey))
{
    throw new Exception("JWT Secret Key is missing in appsettings.json!");
}

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });
builder.Services.AddAuthorization();
var app = builder.Build();

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

