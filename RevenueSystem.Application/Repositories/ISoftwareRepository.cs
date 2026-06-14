using RevenueSystem.Domain.Entities;
namespace RevenueSystem.Application.Repositories;

public interface ISoftwareRepository
{
    Task<Software?> GetSoftwareByIdAsync(int id);
}