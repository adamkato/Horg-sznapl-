using CatchLog.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace CatchLog.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnglersController : ControllerBase
{
    private readonly AppDbContext _db;
    public AnglersController(AppDbContext db) => _db = db;

    // CRUD
    [HttpGet]
    public async Task<ActionResult<List<Angler>>> GetAll() => await _db.Anglers.ToListAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<Angler>> GetById(int id)
    {
        var item = await _db.Anglers.FindAsync(id);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<Angler>> Create(Angler angler)
    {
        _db.Anglers.Add(angler);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = angler.Id }, angler);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Angler angler)
    {
        if (id != angler.Id) return BadRequest();
        _db.Entry(angler).State = EntityState.Modified;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _db.Anglers.FindAsync(id);
        if (item is null) return NotFound();
        _db.Anglers.Remove(item);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("search/{name}")]
    public async Task<ActionResult<List<Angler>>> SearchByName(string name)
    {
        return await _db.Anglers
            .Where(a => a.Name.ToLower().Contains(name.ToLower()))
            .ToListAsync();
    }
}