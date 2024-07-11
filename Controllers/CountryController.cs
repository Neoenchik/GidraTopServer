using GidraTopServer.Data;
using GidraTopServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GidraTopServer.Controllers;

[Route("[controller]")]
public class CountryController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<CountryController> _logger;

    public CountryController(ApplicationDbContext context, ILogger<CountryController> logger)
    {
        _context = context;
        _logger = logger;
    }

        [HttpPost]
    public async Task<IActionResult> Create([FromForm] Country req, [FromForm] FileUpload image)
    {
        if (ModelState.IsValid)
        {
            try
            {
                if (image.files != null && image.files.Length > 0)
                {
                    if (!Directory.Exists("wwwroot/Images/Country"))
                    {
                        Directory.CreateDirectory("wwwroot/Images/Country");
                    }
                    var uniqueFileName = $"{Guid.NewGuid()}_{image.files.FileName}";
                    var imagePath = Path.Combine("wwwroot/Images/Country", uniqueFileName);

                    using var stream = new FileStream(imagePath, FileMode.Create);
                    await image.files.CopyToAsync(stream);
                    stream.Close();

                    req.Img = Path.Combine("/Images/Country", uniqueFileName);
                }
                else
                {
                    _logger.LogWarning("Отсутствует фото страны");
                    return BadRequest("Отсутствует фото страны");
                }
                _context.Countrys.Add(req);
                await _context.SaveChangesAsync();

                return Ok(req);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при сохранении продукта");
                //если ошибка при сохранении
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }
        else
        {
            _logger.LogWarning("Модель недействительна: {ModelState}", ModelState);
            return BadRequest(ModelState);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var countries = await _context.Countrys
                .Include(c => c.Brands)
                .ToListAsync();

        return Ok(countries);
    }
}
