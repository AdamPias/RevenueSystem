using Moq;
using RevenueSystem.Application.Repositories;
using RevenueSystem.Application.Services;

namespace RevenueSystem.Tests;

public class RevenueServiceTests
{
    private readonly Mock<IContractRepository> _contractRepositoryMock;
    private readonly Mock<IExchangeRateService> _exchangeRateServiceMock;
    private readonly RevenueService _revenueService;

    public RevenueServiceTests()
    {
        _contractRepositoryMock = new Mock<IContractRepository>();
        _exchangeRateServiceMock = new Mock<IExchangeRateService>();
        
        _revenueService = new RevenueService(_contractRepositoryMock.Object, _exchangeRateServiceMock.Object);
    }

    [Fact]
    public async Task GetActualRevenueAsync_ShouldReturnPln_WhenNoCurrencyProvided()
    {
        decimal expectedRevenue = 5000m;
        _contractRepositoryMock.Setup(repo => repo.CalculateActualRevenueAsync(null))
            .ReturnsAsync(expectedRevenue);
        
        var result = await _revenueService.GetActualRevenueAsync(null, null);
        
        Assert.Equal(expectedRevenue, result);
        _exchangeRateServiceMock.Verify(x => x.GetExchangeRateAsync(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task GetActualRevenueAsync_ShouldConvertCurrency_WhenCurrencyCodeProvided()
    {
        decimal revenueInPln = 5000m;
        string targetCurrency = "USD";
        decimal usdExchangeRate = 4.0m;
        decimal expectedConvertedRevenue = 1250m;

        _contractRepositoryMock.Setup(repo => repo.CalculateActualRevenueAsync(null))
            .ReturnsAsync(revenueInPln);
        
        _exchangeRateServiceMock.Setup(exchange => exchange.GetExchangeRateAsync(targetCurrency))
            .ReturnsAsync(usdExchangeRate);
        
        var result = await _revenueService.GetActualRevenueAsync(null, targetCurrency);
        
        Assert.Equal(expectedConvertedRevenue, result);
        _exchangeRateServiceMock.Verify(x => x.GetExchangeRateAsync(targetCurrency), Times.Once);
    }
}