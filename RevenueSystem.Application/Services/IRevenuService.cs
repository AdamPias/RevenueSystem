namespace RevenueSystem.Application.Services;

public interface IRevenueService
{
    Task<decimal> GetActualRevenueAsync(int? softwareId, string? currencyCode);
    Task<decimal> GetProjectedRevenueAsync(int? softwareId, string? currencyCode);
}