namespace CatchLog.Wpf.Models;

public class Angler
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string LicenseNumber { get; set; } = string.Empty;
}

public class Spot
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string WaterType { get; set; } = string.Empty;
}

public class CatchRecord
{
    public int Id { get; set; }
    public string Species { get; set; } = string.Empty;
    public double WeightKg { get; set; }
    public DateTime CaughtAt { get; set; } = DateTime.Now;
    public int AnglerId { get; set; }
    public Angler? Angler { get; set; }
    public int SpotId { get; set; }
    public Spot? Spot { get; set; }
}