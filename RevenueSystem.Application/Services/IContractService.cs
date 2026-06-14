using RevenueSystem.Application.DTOs;

namespace RevenueSystem.Application.Services;

public interface IContractService
{
    Task<int> CreateContractAsync(CreateContractDto dto);
    Task ProcessPaymentAsync(int contractId, CreatePaymentDto dto);
    
}