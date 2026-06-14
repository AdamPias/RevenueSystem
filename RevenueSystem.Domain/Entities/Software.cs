using System.Globalization;

namespace RevenueSystem.Domain.Entities;

public class Software
{
    public int Id { get; init; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string CurrentVersion { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal YearlyCost { get; set; }
    
}