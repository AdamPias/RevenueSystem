using RevenueSystem.Domain.Entities;

namespace RevenueSystem.Application.Repositories;

public interface IContractRepository
{
    Task<bool> HasActiveContractForSoftwareAsync(int clientId, int softwareId);
    Task<bool> IsReturningClientAsync(int clientId);
    Task<int> AddContractAsync(Contract contract);
    Task<Contract?> GetContractWithPaymentsAsync(int contractId);
    Task UpdateContractAsync(Contract contract);
    Task<decimal> CalculateActualRevenueAsync(int? softwareId);
    Task<decimal> CalculateProjectedRevenueAsync(int? softwareId);
}