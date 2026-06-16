using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RevenueSystem.Application.Services;

namespace RevenueSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RevenueController : ControllerBase
{
    private readonly IRevenueService _revenueService;

    public RevenueController(IRevenueService revenueService)
    {
        _revenueService = revenueService;
    }

    [HttpGet("actual")]
    public async Task<IActionResult> GetActualRevenue([FromQuery] int? softwareId, [FromQuery] string? currencyCode)
    {
        var revenue = await _revenueService.GetActualRevenueAsync(softwareId, currencyCode);
        return Ok(new { Revenue = revenue, Currency = string.IsNullOrWhiteSpace(currencyCode) ? "PLN" : currencyCode.ToUpper() });
    }

    [HttpGet("projected")]
    public async Task<IActionResult> GetProjectedRevenue([FromQuery] int? softwareId, [FromQuery] string? currencyCode)
    {
        var revenue = await _revenueService.GetProjectedRevenueAsync(softwareId, currencyCode);
        return Ok(new { Revenue = revenue, Currency = string.IsNullOrWhiteSpace(currencyCode) ? "PLN" : currencyCode.ToUpper() });
    }
}