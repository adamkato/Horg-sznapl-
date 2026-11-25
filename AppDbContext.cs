using CatchLog.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace CatchLog;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Angler> Anglers => Set<Angler>();
    public DbSet<Spot> Spots => Set<Spot>();
    public DbSet<CatchRecord> Catches => Set<CatchRecord>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Angler>().HasData(
            new Angler { Id = 1, Name = "Kovács János", LicenseNumber = "HU-112233" },
            new Angler { Id = 2, Name = "Nagy Péter", LicenseNumber = "HU-998877" }
        );

        modelBuilder.Entity<Spot>().HasData(
            new Spot { Id = 1, Name = "Ráckevei-Soroksári Duna", WaterType = "Folyó" },
            new Spot { Id = 2, Name = "Balaton - Siófok", WaterType = "Tó" }
        );

        modelBuilder.Entity<CatchRecord>().HasData(
            new CatchRecord { Id = 1, Species = "Ponty", WeightKg = 6.5, AnglerId = 1, SpotId = 1, CaughtAt = DateTime.Now.AddDays(-2) },
            new CatchRecord { Id = 2, Species = "Csuka", WeightKg = 3.2, AnglerId = 2, SpotId = 2, CaughtAt = DateTime.Now.AddDays(-1) }
        );
    }
}