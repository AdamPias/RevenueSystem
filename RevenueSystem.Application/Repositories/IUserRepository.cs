using RevenueSystem.Domain.Entities;

namespace RevenueSystem.Application.Repositories;

public interface IUserRepository
{
    Task<User?> GetUserByLoginAsync(string login);
    Task<User?> GetUserByRefreshTokenAsync(string refreshToken);
    Task UpdateUserAsync(User user);
}