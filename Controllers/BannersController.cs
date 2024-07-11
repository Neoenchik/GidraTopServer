using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GidraTopServer.Data;
using GidraTopServer.Models;

namespace GidraTopServer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BannersController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public BannersController(ApplicationDbContext context)
    {
        _context = context;
    }


    //получаем все баннеры
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Banner>>> GetBanner()
    {
        return await _context.Banner.ToListAsync();
    }

    //получаем баннер по id
    [HttpGet("{id}")]
    public async Task<ActionResult<Banner>> GetBanner(int id)
    {
        var banner = await _context.Banner.FindAsync(id);

        if (banner == null)
        {
            return NotFound();
        }

        return banner;
    }


    // изменяем существующий баннер
    [HttpPut("{id}")]
    public async Task<IActionResult> PutBanner(int id, Banner banner)
    {
        if (id != banner.Id)//от случайного обновления неправильного баннера
        {
            return BadRequest();
        }

        _context.Entry(banner).State = EntityState.Modified;//говорит ef, что нужно обновить данные баннера в бд

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!BannerExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // добавить новый баннер
    [HttpPost]
    public async Task<ActionResult<Banner>> PostBanner(Banner banner)
    {
        _context.Banner.Add(banner);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetBanner", new { id = banner.Id }, banner);
    }

    // удалить баннер по id
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBanner(int id)
    {
        var banner = await _context.Banner.FindAsync(id);
        if (banner == null)
        {
            return NotFound();
        }

        _context.Banner.Remove(banner);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    //проверка на существование баннера с таким id
    private bool BannerExists(int id)
    {
        return _context.Banner.Any(e => e.Id == id);
    }
}
