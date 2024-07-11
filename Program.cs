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

// ��������� ���������� �����
var secretKeyBytes = new byte[32]; // 256 ���
using (var rng = new RNGCryptoServiceProvider())
{
    rng.GetBytes(secretKeyBytes);
}
var secretKey = Convert.ToBase64String(secretKeyBytes);

// ����������� �������� � ���������
var issuer = "https://your-app.com"; // �������� �� ��� URL ��� ������������� ����������
var audience = "https://your-app.com/users"; // �������� �� ��� ������������� ���������


var key = Encoding.ASCII.GetBytes(secretKey); //�������� ������ �� ����� ��� ������� jwt �������
builder.Services.AddAuthentication(options => //��������� ������ �������������� � ����������
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; 
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})//���������� ����� �������������� �� ���������
.AddJwtBearer(options => //��������� ��������� jwt bearer ��������������
{
    options.RequireHttpsMetadata = true; //���������� https ��� ����������
    options.SaveToken = true; //��������� ���������� ������ � httpContext
    options.TokenValidationParameters = new TokenValidationParameters //��������� ��������� ������
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = issuer, //���������� ����������� ��������
        ValidAudience = audience, // � ���������
        IssuerSigningKey = new SymmetricSecurityKey(key) //������������� ���� ��� �������
    };
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//��������� Cors
builder.Services.AddCors(options=>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:3000")
        .AllowAnyHeader()
        .AllowAnyMethod();//���������� �������� � ������
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
