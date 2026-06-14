namespace RevenueSystem.Domain.Entities;

public class IndividualClient : Client
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Pesel { get; init; } = string.Empty;
    public bool isDeleted { get; set; } = false;

}