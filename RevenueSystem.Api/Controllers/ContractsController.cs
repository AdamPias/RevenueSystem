using Microsoft.AspNetCore.Mvc;
using RevenueSystem.Application.DTOs;
using RevenueSystem.Application.Services;

namespace RevenueSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContractsController : ControllerBase
{
    private readonly IContractService _contractService;

    public ContractsController(IContractService contractService)
    {
        _contractService = contractService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateContract([FromBody] CreateContractDto dto)
    {
        try
        {
            var contractId = await _contractService.CreateContractAsync(dto);
            return Ok(new { Message = "Oferta/Kontrakt został przygotowany.", ContractId = contractId });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }
    
    [HttpPost("{contractId:int}/payments")]
    public async Task<IActionResult> MakePayment(int contractId, [FromBody] CreatePaymentDto dto)
    {
        try
        {
            await _contractService.ProcessPaymentAsync(contractId, dto);
            return Ok(new { Message = "Płatność została zarejestrowana pomyślnie." });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }
}