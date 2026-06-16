using RevenueSystem.Application.DTOs;
using RevenueSystem.Application.Repositories;
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
    
    public async Task DeleteClientAsync(int id)
    {
        var client = await _repository.GetClientByIdAsync(id);
    
        if (client == null)
            throw new ArgumentException("Klient nie istnieje.");
        
        if (client is CorporateClient)
            throw new ArgumentException("Zgodnie z zasadami, nie można usuwać danych firm.");
        
        if (client is IndividualClient individualClient)
        {
            if (individualClient.IsDeleted)
                throw new ArgumentException("Klient nie istnieje (został już usunięty).");

            individualClient.IsDeleted = true; 
            await _repository.UpdateAsync(individualClient);
        }
    }

    public async Task UpdateClientAsync(int id, UpdateClientDto dto)
    {
        var client = await _repository.GetClientByIdAsync(id);
    
        if (client == null)
            throw new ArgumentException("Klient nie istnieje.");
        
        if (client is IndividualClient individualClient && individualClient.IsDeleted)
            throw new ArgumentException("Klient nie istnieje.");
        
        client.Email = dto.Email;
        client.PhoneNumber = dto.PhoneNumber;
        client.Address = dto.Address;
        
        if (client is IndividualClient indClient)
        {
            if (!string.IsNullOrEmpty(dto.FirstName))
                indClient.FirstName = dto.FirstName;
        
            if (!string.IsNullOrEmpty(dto.LastName))
                indClient.LastName = dto.LastName;
        }

        await _repository.UpdateAsync(client);
    }
}