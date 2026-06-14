namespace RevenueSystem.Application.Repositories;

public interface IDiscountRepository
{
    Task<decimal> GetHighestActiveDiscountAsync(DateTime targetDate);
}