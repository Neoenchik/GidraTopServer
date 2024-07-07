using GidraTopServer.Data;
using GidraTopServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GidraTopServer.Controllers;

[Route("[controller]")]
public class BrandController : Controller
{
    private readonly ApplicationDbContext _context;

    public BrandController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Brand req)
    {
        if (ModelState.IsValid)
        {
            try
            {
                _context.Brands.Add(req);
                await _context.SaveChangesAsync();

                return Ok(req);
            }
            catch (Exception ex)
            {
                //если ошибка при сохранении
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }
        else
        {
            return BadRequest(ModelState);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var brand = await _context.Brands.ToListAsync();
        return Ok(brand);
    }
}
