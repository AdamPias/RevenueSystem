using RevenueSystem.Application.Repositories;

namespace RevenueSystem.Application.Services;

public class RevenueService : IRevenueService
{
    private readonly IContractRepository _contractRepository;
    private readonly IExchangeRateService _exchangeRateService;

    public RevenueService(IContractRepository contractRepository, IExchangeRateService exchangeRateService)
    {
        _contractRepository = contractRepository;
        _exchangeRateService = exchangeRateService;
    }

    public async Task<decimal> GetActualRevenueAsync(int? softwareId, string? currencyCode)
    {
        var revenueInPln = await _contractRepository.CalculateActualRevenueAsync(softwareId);
        return await ConvertCurrencyAsync(revenueInPln, currencyCode);
    }

    public async Task<decimal> GetProjectedRevenueAsync(int? softwareId, string? currencyCode)
    {
        var projectedRevenueInPln = await _contractRepository.CalculateProjectedRevenueAsync(softwareId);
        return await ConvertCurrencyAsync(projectedRevenueInPln, currencyCode);
    }

    private async Task<decimal> ConvertCurrencyAsync(decimal amountInPln, string? currencyCode)
    {
        if (string.IsNullOrWhiteSpace(currencyCode) || currencyCode.Equals("PLN", StringComparison.OrdinalIgnoreCase))
            return amountInPln;

        // Pobieramy kurs waluty z NBP
        var exchangeRate = await _exchangeRateService.GetExchangeRateAsync(currencyCode);
        
        return Math.Round(amountInPln / exchangeRate, 2);
    }
}