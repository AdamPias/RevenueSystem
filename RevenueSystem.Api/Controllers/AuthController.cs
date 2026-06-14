using Microsoft.AspNetCore.Mvc;
using RevenueSystem.Application.DTOs;
using RevenueSystem.Application.Services;

namespace RevenueSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        try
        {
            var tokens = await _authService.LoginAsync(dto);
            return Ok(tokens);
        }
        catch (ArgumentException ex)
        {
            return Unauthorized(new { Error = ex.Message });
        }
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshDto dto)
    {
        try
        {
            var tokens = await _authService.RefreshTokensAsync(dto);
            return Ok(tokens);
        }
        catch (ArgumentException ex)
        {
            return Unauthorized(new { Error = ex.Message });
        }
    }
}