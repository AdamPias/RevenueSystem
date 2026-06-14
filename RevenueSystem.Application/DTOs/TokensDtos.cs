namespace RevenueSystem.Application.DTOs;

public record TokensDto(string AccessToken, string RefreshToken);
public record RefreshDto(string RefreshToken);