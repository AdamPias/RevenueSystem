using Microsoft.AspNetCore.Mvc;
using RevenueSystem.Application.DTOs;
using RevenueSystem.Application.Services;

namespace RevenueSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientsController : ControllerBase
{
    private readonly IClientService _clientService;
    
    public ClientsController(IClientService clientService)
    {
        _clientService = clientService;
    }

    [HttpPost("individual")]
    public async Task<IActionResult> AddIndividualClient([FromBody] CreateIndividualClientDto dto)
    {
        try
        {
            var clientId = await _clientService.AddIndividualClientAsync(dto);
            return Ok(new { Message = "Dodano klienta indywidualnego", ClientId = clientId });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }

    [HttpPost("corporate")]
    public async Task<IActionResult> AddCorporateClient([FromBody] CreateCorporateClientDto dto)
    {
        try
        {
            var clientId = await _clientService.AddCorporateClientAsync(dto);
            return Ok(new { Message = "Dodano klienta firmowego", ClientId = clientId });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }
}