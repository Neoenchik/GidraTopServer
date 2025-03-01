﻿using GidraTopServer.Data;
using GidraTopServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GidraTopServer.Controllers;

[Route("[controller]")]
public class ProductController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ProductController> _logger;

    public ProductController(ApplicationDbContext context, ILogger<ProductController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] Product req, [FromForm] FileUpload image)
    {
        if (ModelState.IsValid)
        {
            try
            {
                if (image != null && image.files.Length > 0)
                {
                    if (!Directory.Exists("wwwroot/Images/Product"))
                    {
                        Directory.CreateDirectory("wwwroot/Images/Product");
                    }
                    var uniqueFileName = $"{Guid.NewGuid()}_{image.files.FileName}";
                    var imagePath = Path.Combine("wwwroot/Images/Product", uniqueFileName);

                    using var stream = new FileStream(imagePath, FileMode.Create);
                    await image.files.CopyToAsync(stream);
                    stream.Close();

                    req.Img = Path.Combine("/Images/Country", uniqueFileName);
                }
                else
                {
                    _logger.LogWarning("Отсутствует фото для продукта");
                    return BadRequest("Отсутствует фото для продукта");
                }
                _context.Products.Add(req);
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
    public async Task<IActionResult> GetAll(int? brandId, int? categoryId)
    {
        IQueryable<Product> query = _context.Products;

        if (brandId.HasValue)
        {
            query = query.Where(p => p.BrandId == brandId);
        }

        if (categoryId.HasValue)
        {
            query = query.Where(p => p.CategoryId == categoryId);
        }

        var products = await query.ToListAsync();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOne(int id)
    {
        var product = await _context.Products.FindAsync(id);

        return (product==null)?NotFound():Ok(product);
    }
}
