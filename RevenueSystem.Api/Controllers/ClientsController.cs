using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RevenueSystem.Application.DTOs;
using RevenueSystem.Application.Services;

namespace RevenueSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
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
    
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteClient(int id)
    {
        try
        {
            await _clientService.DeleteClientAsync(id);
            return Ok(new { Message = "Klient został usunięty (Soft Delete)." });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateClient(int id, [FromBody] UpdateClientDto dto)
    {
        try
        {
            await _clientService.UpdateClientAsync(id, dto);
            return Ok(new { Message = "Dane klienta zostały zaktualizowane." });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }
}
