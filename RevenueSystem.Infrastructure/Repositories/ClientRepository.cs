using Microsoft.EntityFrameworkCore;
using RevenueSystem.Application.Repositories;
using RevenueSystem.Domain.Entities;

namespace RevenueSystem.Infrastructure.Repositories;

public class ClientRepository : IClientRepository
{
    private readonly AppDbContext _context;

    public ClientRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> IsPeselTakenAsync(string pesel)
    {
        return await _context.Clients.OfType<IndividualClient>()
            .AnyAsync(c => c.Pesel == pesel);
    }

    public async Task<bool> IsKrsTakenAsync(string krs)
    {
        return await _context.Clients.OfType<CorporateClient>()
            .AnyAsync(c => c.Krs == krs);
    }

    public async Task<int> AddClientAsync(Client client)
    {
        await _context.Clients.AddAsync(client);
        await _context.SaveChangesAsync();
        return client.Id;
    }

    public async Task<Client?> GetClientByIdAsync(int id)
    {
        return await _context.Clients.FirstOrDefaultAsync(c => c.Id == id);
    }
    
    public async Task UpdateAsync(Client client)
    {
        _context.Clients.Update(client);
        await _context.SaveChangesAsync();
    }
}