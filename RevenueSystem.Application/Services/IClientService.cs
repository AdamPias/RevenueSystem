using RevenueSystem.Application.DTOs;

namespace RevenueSystem.Application.Services;

public interface IClientService
{
    Task<int> AddIndividualClientAsync(CreateIndividualClientDto dto);
    Task<int> AddCorporateClientAsync(CreateCorporateClientDto dto);    
    Task DeleteClientAsync(int id);
    Task UpdateClientAsync(int id, UpdateClientDto dto);
}