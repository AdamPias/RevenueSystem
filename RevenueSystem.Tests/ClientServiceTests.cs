using Moq;
using RevenueSystem.Application.Repositories;
using RevenueSystem.Application.Services;
using RevenueSystem.Domain.Entities;

namespace RevenueSystem.Tests;

public class ClientServiceTests
{
    private readonly Mock<IClientRepository> _clientRepositoryMock;
    private readonly ClientService _clientService;

    public ClientServiceTests()
    {
        _clientRepositoryMock = new Mock<IClientRepository>();
        _clientService = new ClientService(_clientRepositoryMock.Object);
    }

    [Fact]
    public async Task DeleteClientAsync_WhenClientIsCorporate()
    {
        int corporateClientId = 99;
        
        var fakeCorporateClient = new CorporateClient 
        { 
            Id = corporateClientId, 
            CompanyName = "Testowa Firma Sp. z o.o." 
        };
        
        _clientRepositoryMock.Setup(repo => repo.GetClientByIdAsync(corporateClientId))
            .ReturnsAsync(fakeCorporateClient);
        
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => 
            _clientService.DeleteClientAsync(corporateClientId)
        );
        
        Assert.Equal("Zgodnie z zasadami, nie można usuwać danych firm.", exception.Message);
    }

    [Fact]
    public async Task DeleteClientAsync_WhenClientDoesNotExist()
    {
        int nonExistentClientId = 404;
        
        _clientRepositoryMock.Setup(repo => repo.GetClientByIdAsync(nonExistentClientId))
            .ReturnsAsync((Client?)null);
        
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => 
            _clientService.DeleteClientAsync(nonExistentClientId)
        );

        Assert.Equal("Klient nie istnieje.", exception.Message);
    }
}