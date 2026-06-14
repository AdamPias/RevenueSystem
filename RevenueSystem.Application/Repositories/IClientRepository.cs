using RevenueSystem.Domain.Entities;

namespace RevenueSystem.Application.Repositories;

public interface IClientRepository
{
    Task<bool> IsPeselTakenAsync(string pesel);
    Task<bool> IsKrsTakenAsync(string krs);
    Task<int> AddClientAsync(Client client);
}