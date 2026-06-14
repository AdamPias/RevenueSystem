namespace RevenueSystem.Application.Services;

public interface IExchangeRateService
{
    Task<decimal> GetExchangeRateAsync(string currencyCode);
}