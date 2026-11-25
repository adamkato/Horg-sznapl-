using CatchLog.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace CatchLog.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CatchesController : ControllerBase
{
    private readonly AppDbContext _db;
    public CatchesController(AppDbContext db) => _db = db;

    // CRUD
    [HttpGet]
    public async Task<ActionResult<List<CatchRecord>>> GetAll()
    {
        return await _db.Catches
            .Include(c => c.Angler)
            .Include(c => c.Spot)
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CatchRecord>> GetById(int id)
    {
        var item = await _db.Catches.Include(c => c.Angler).Include(c => c.Spot).FirstOrDefaultAsync(c => c.Id == id);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<CatchRecord>> Create(CatchRecord record)
    {
        record.Angler = null;
        record.Spot = null;
        _db.Catches.Add(record);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = record.Id }, record);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, CatchRecord record)
    {
        if (id != record.Id) return BadRequest();
        _db.Entry(record).State = EntityState.Modified;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _db.Catches.FindAsync(id);
        if (item is null) return NotFound();
        _db.Catches.Remove(item);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("biggest")]
    public async Task<ActionResult<CatchRecord>> GetBiggestCatch()
    {
        var biggest = await _db.Catches
            .Include(c => c.Angler)
            .Include(c => c.Spot)
            .OrderByDescending(c => c.WeightKg)
            .FirstOrDefaultAsync();

        return biggest is null ? NotFound() : Ok(biggest);
    }

    [HttpGet("total-weight/{anglerId}")]
    public async Task<ActionResult<double>> GetTotalWeightByAngler(int anglerId)
    {
        var total = await _db.Catches
            .Where(c => c.AnglerId == anglerId)
            .SumAsync(c => c.WeightKg);

        return Ok(Math.Round(total, 2));
    }
}