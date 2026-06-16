namespace RevenueSystem.Application.DTOs;

public record UpdateClientDto(
    string? Email,
    string? PhoneNumber,
    string? Address
);