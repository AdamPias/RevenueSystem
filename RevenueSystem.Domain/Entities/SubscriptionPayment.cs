namespace RevenueSystem.Domain.Entities;

public class SubscriptionPayment
{
    public int Id { get; init; }
    public int SubscriptionId { get; init; }
    public decimal Amount { get; init; }
    public DateTime PaymentDate { get; init; }
    
    public Subscription Subscription { get; init; } = null!;

}