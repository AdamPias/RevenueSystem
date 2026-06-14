using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RevenueSystem.Application.DTOs;
using RevenueSystem.Application.Repositories;
using RevenueSystem.Domain.Entities;

namespace RevenueSystem.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;

    public AuthService(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }

    public async Task<TokensDto> LoginAsync(LoginDto dto)
    {
        var user = await _userRepository.GetUserByLoginAsync(dto.Login);
        
        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            throw new ArgumentException("Nieprawidłowy login lub hasło.");

        return await GenerateAndSaveTokensAsync(user);
    }

    public async Task<TokensDto> RefreshTokensAsync(RefreshDto dto)
    {
        var user = await _userRepository.GetUserByRefreshTokenAsync(dto.RefreshToken);
        
        if (user is null)
            throw new ArgumentException("Nieprawidłowy refresh token.");

        if (user.RefreshTokenExpiresAt < DateTime.UtcNow)
            throw new ArgumentException("Refresh token wygasł. Zaloguj się ponownie.");

        return await GenerateAndSaveTokensAsync(user);
    }

    private async Task<TokensDto> GenerateAndSaveTokensAsync(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Login),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var jwt = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(15), 
            signingCredentials: creds
        );

        var accessToken = new JwtSecurityTokenHandler().WriteToken(jwt);
        var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(7);
        
        // Zapisujemy zmiany przez repozytorium
        await _userRepository.UpdateUserAsync(user);

        return new TokensDto(accessToken, refreshToken);
    }
}