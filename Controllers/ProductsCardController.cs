using GidraTopServer.Data;
using GidraTopServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GidraTopServer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsCardController : ControllerBase
{
    //сервис или репозиторий для работы с товарами
    private readonly ApplicationDbContext _context;
    public ProductsCardController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet()]
    public async Task<ActionResult<IEnumerable<ProductCard>>> GetProductCards()
    {
        return await _context.ProductCards.ToListAsync();
    }

    //Другие методы API для CRUD операций

}
