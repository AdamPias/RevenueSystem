namespace RevenueSystem.Domain.Entities;

public class Subscription
{
    public int Id { get; init; }
    public int ClientId { get; init; }
    public int SoftwareId { get; init; }
    public string Name { get; set; } = string.Empty;
    public int RenewalPeriodMonths { get; set; } 
    public decimal Price { get; set; }
    public bool IsActive { get; set; } = true;
    
    public Client Client { get; init; } = null!;
    public Software Software { get; init; } = null!;
    public List<SubscriptionPayment> Payments { get; set; } = new();
}