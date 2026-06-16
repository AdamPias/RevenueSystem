namespace RevenueSystem.Application.DTOs;

public class UpdateClientDto
{
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
    
    public string? FirstName { get; set; } 
    public string? LastName { get; set; }
}