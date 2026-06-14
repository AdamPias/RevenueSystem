namespace RevenueSystem.Domain.Entities;

public class Payment
{
    public int Id { get; init; }
    public int ContractId { get; init; }
    public decimal Amount {get; set; }
    public DateTime PaymentDate { get; set; }
    public bool IsRefunded { get; set; } = false;

    public Contract Contract { get; init; } = null!;
}