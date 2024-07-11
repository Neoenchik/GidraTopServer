using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GidraTopServer.Data;
using GidraTopServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace GidraTopServer.Controllers;

[Route("[controller]")]
public class UserController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    public UserController(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    [HttpPost("registration")]
    public async Task<IActionResult> Register(User model)
    {
        if (ModelState.IsValid)
        {
            //проверяем телефон и почту на то, сущетсвует ли она уже в бд
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email || u.PhoneNumber == model.PhoneNumber);

            if (existingUser != null)
            {
                ModelState.AddModelError("", "Аккаунт с такой почтой или номером телефона уже создан");
                return View(model);
            }
            //хешируем пароль
            model.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);

            //добавляем пользователя в бд
            await _context.Users.AddAsync(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("Login");
        }
        else
        {
            return BadRequest("Некорректные данные");
        }
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody]LoginModel model)
    {
        //Проверка валидности данных модели
        if (!ModelState.IsValid)
        {
            return BadRequest("Некорректные данные");
        }

        // Поиск пользователя в базе данных по email
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == model.Email);

        // Проверка существования пользователя и совпадения пароля
        if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
        {
            return Unauthorized("Некорректный email или пароль");
        }

        // Генерация JWT токена
        var token = GenerateJwtToken(user);

        //возврат токена клиенту
        return Ok(new { token });
    }

    private string GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();//нужен для создания и обработки jwt токенов
        var key = Encoding.ASCII.GetBytes(s: _configuration["Jwt:Key"]); //получение секретного ключа и преобразование его в массив байтов
        var tokenDescriptor = new SecurityTokenDescriptor //создание описателя токенов, информации о токене
        {
            Subject = new ClaimsIdentity(new Claim[] //определение идентичности токена
            {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()), //указвается идентификатор пользователя
            new(ClaimTypes.Email, user.Email),//почта
            new(ClaimTypes.Role, user.Role)//роль
            }),
            Expires = DateTime.UtcNow.AddDays(7),//время жизни токена 7 дней
            //установка подписывающих учетных данных, используя симметричный ключ и алгоритм
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature), 
            Issuer = _configuration["Jwt:Issuer"], //установка издателя токена
            Audience = _configuration["Jwt:Audience"] //аудитория токена
        };
        var token = tokenHandler.CreateToken(tokenDescriptor); //создание токена
        return tokenHandler.WriteToken(token); //в виде втроки возврат
    }
}

public class LoginModel
{
    [Required]
    [EmailAddress]
    public required string Email { get; set; }

    [Required]
    public required string Password { get; set; }
}