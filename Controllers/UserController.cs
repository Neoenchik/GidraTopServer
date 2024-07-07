using GidraTopServer.Data;
using GidraTopServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GidraTopServer.Controllers;

[Route("[controller]")]
public class UserController : Controller
{
    private readonly ApplicationDbContext _context;

    public UserController(ApplicationDbContext context)
        { _context = context; }

    [HttpPost("registration")]
    public async Task<IActionResult> Register(User model)
    {
        if (ModelState.IsValid)
        {
            //проверяем телефон и почту на то, сущетсвует ли она уже в бд
            var existingUser =await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email || u.PhoneNumber == model.PhoneNumber);

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

        return View(model);
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login(User model)
    {
        return View(model);
    }

    [HttpGet("auth")]
    public async Task<IActionResult> Check([FromQuery] int? id)
    {
        if (!id.HasValue)
        {
            return BadRequest("Не задан id");
        }

        return Ok(new { Id = id });
    }
}
