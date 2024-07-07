using GidraTopServer.Data;
using GidraTopServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Ocsp;

namespace GidraTopServer.Controllers;

[Route("[controller]")]
public class ProductController : Controller
{
    private readonly ApplicationDbContext _context;

    public ProductController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Create(Product req)
    {
        if (ModelState.IsValid)
        {
            try
            {
                _context.Products.Add(req);
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
        var product = await _context.Products.ToListAsync();
        return Ok(product);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOne(int id)
    {
        var product = await _context.Products.FindAsync(id);

        return (product==null)?NotFound():Ok(product);
    }
}
