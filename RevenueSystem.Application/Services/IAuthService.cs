using RevenueSystem.Application.DTOs;

namespace RevenueSystem.Application.Services;

public interface IAuthService
{
    Task<TokensDto> LoginAsync(LoginDto dto);
    Task<TokensDto> RefreshTokensAsync(RefreshDto dto);
}