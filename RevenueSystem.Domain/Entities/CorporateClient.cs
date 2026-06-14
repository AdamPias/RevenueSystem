namespace RevenueSystem.Domain.Entities;

public class CorporateClient : Client
{
    public string CompanyName { get; set; } = string.Empty;
    public string Krs { get; init; } = string.Empty;

}