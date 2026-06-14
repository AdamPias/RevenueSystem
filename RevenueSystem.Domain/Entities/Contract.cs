namespace RevenueSystem.Domain.Entities;

public class Contract
{
    public int Id { get; init; }
    public int ClientId { get; init; }
    public int SoftwareId { get; init; }
    public string SoftwareVersion { get; init; } = string.Empty;
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public decimal TotalPrice { get; init; }
    public int ExtraSupportYears { get; init; }
    public bool IsSigned { get; set; } = false;

    public Client Client { get; init; } = null!;
    public Software Software { get; init; } = null!;
    public List<Payment> Payments { get; set; } = new();
}