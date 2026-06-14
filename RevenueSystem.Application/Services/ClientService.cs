using RevenueSystem.Application.DTOs;
using RevenueSystem.Application.Repositories; // Nowy using
using RevenueSystem.Domain.Entities;

namespace RevenueSystem.Application.Services;

public class ClientService : IClientService
{
    private readonly IClientRepository _repository;

    public ClientService(IClientRepository repository)
    {
        _repository = repository;
    }

    public async Task<int> AddIndividualClientAsync(CreateIndividualClientDto dto)
    {
        var exists = await _repository.IsPeselTakenAsync(dto.Pesel);
        if (exists)
            throw new ArgumentException("Klient z podanym numerem PESEL już istnieje w systemie.");

        var client = new IndividualClient
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Address = dto.Address,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            Pesel = dto.Pesel
        };

        return await _repository.AddClientAsync(client);
    }

    public async Task<int> AddCorporateClientAsync(CreateCorporateClientDto dto)
    {
        var exists = await _repository.IsKrsTakenAsync(dto.Krs);
        if (exists)
            throw new ArgumentException("Firma z podanym numerem KRS już istnieje w systemie.");

        var client = new CorporateClient
        {
            CompanyName = dto.CompanyName,
            Address = dto.Address,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            Krs = dto.Krs
        };

        return await _repository.AddClientAsync(client);
    }
}