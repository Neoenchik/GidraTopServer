using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using GidraTopServer.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();

// Генерация секретного ключа
var secretKeyBytes = new byte[32]; // 256 бит
using (var rng = new RNGCryptoServiceProvider())
{
    rng.GetBytes(secretKeyBytes);
}
var secretKey = Convert.ToBase64String(secretKeyBytes);

// Определение издателя и аудитории
var issuer = "https://your-app.com"; // Замените на ваш URL или идентификатор приложения
var audience = "https://your-app.com/users"; // Замените на ваш идентификатор аудитории


var key = Encoding.ASCII.GetBytes(secretKey); //байтовый массив из ключа для подписи jwt токенов
builder.Services.AddAuthentication(options => //добавляем службу аутентификации в приложение
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; 
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})//установили схему аутентификации по умолчанию
.AddJwtBearer(options => //добавляет поддержку jwt bearer аутентификации
{
    options.RequireHttpsMetadata = true; //требование https для метаданных
    options.SaveToken = true; //разрешает сохранение токена в httpContext
    options.TokenValidationParameters = new TokenValidationParameters //параметры валидации токена
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = issuer, //определяет допустимого издателя
        ValidAudience = audience, // и аудиторию
        IssuerSigningKey = new SymmetricSecurityKey(key) //Устанавливает ключ для подписи
    };
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//поддержка Cors
builder.Services.AddCors(options=>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:3000")
        .AllowAnyHeader()
        .AllowAnyMethod();//разрешение запросов с фронта
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseRouting();

app.UseStaticFiles();

app.UseCors();

app.MapControllers();

app.Run();
