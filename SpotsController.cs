using CatchLog.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace CatchLog.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SpotsController : ControllerBase
{
    private readonly AppDbContext _db;
    public SpotsController(AppDbContext db) => _db = db;

    // CRUD
    [HttpGet]
    public async Task<ActionResult<List<Spot>>> GetAll() => await _db.Spots.ToListAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<Spot>> GetById(int id)
    {
        var item = await _db.Spots.FindAsync(id);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<Spot>> Create(Spot spot)
    {
        _db.Spots.Add(spot);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = spot.Id }, spot);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Spot spot)
    {
        if (id != spot.Id) return BadRequest();
        _db.Entry(spot).State = EntityState.Modified;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _db.Spots.FindAsync(id);
        if (item is null) return NotFound();
        _db.Spots.Remove(item);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("by-watertype/{waterType}")]
    public async Task<ActionResult<List<Spot>>> GetByWaterType(string waterType)
    {
        return await _db.Spots
            .Where(s => s.WaterType.ToLower() == waterType.ToLower())
            .ToListAsync();
    }
}