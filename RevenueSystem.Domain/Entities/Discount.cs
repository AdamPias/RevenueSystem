namespace RevenueSystem.Domain.Entities;

public class Discount
{
    public int Id { get; init; }
    public string Name { get; set; } = string.Empty;
    public decimal ValuePercentage { get; set; }
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
}