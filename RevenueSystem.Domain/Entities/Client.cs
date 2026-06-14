namespace RevenueSystem.Domain.Entities;

public abstract class Client
{
    public int Id { get; init; }
    public string Address { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
}