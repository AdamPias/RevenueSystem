using Microsoft.EntityFrameworkCore;
using RevenueSystem.Application.Repositories;
using RevenueSystem.Domain.Entities;

namespace RevenueSystem.Infrastructure.Repositories;

public class ContractRepository : IContractRepository
{
    private readonly AppDbContext _context;
    public ContractRepository(AppDbContext context) => _context = context;

    public async Task<bool> HasActiveContractForSoftwareAsync(int clientId, int softwareId)
    {
        return await _context.Contracts
            .AnyAsync(c => c.ClientId == clientId && c.SoftwareId == softwareId && c.IsSigned);
    }

    public async Task<bool> IsReturningClientAsync(int clientId)
    {
        return await _context.Contracts
            .AnyAsync(c => c.ClientId == clientId && c.IsSigned);
    }

    public async Task<int> AddContractAsync(Contract contract)
    {
        await _context.Contracts.AddAsync(contract);
        await _context.SaveChangesAsync();
        return contract.Id;
    }
    
    public async Task<Contract?> GetContractWithPaymentsAsync(int contractId)
    {
        return await _context.Contracts
            .Include(c => c.Payments)
            .FirstOrDefaultAsync(c => c.Id == contractId);
    }

    public async Task UpdateContractAsync(Contract contract)
    {
        _context.Contracts.Update(contract);
        await _context.SaveChangesAsync();
    }
    
    public async Task<decimal> CalculateActualRevenueAsync(int? softwareId)
    {
        var query = _context.Contracts.Where(c => c.IsSigned);
        
        if (softwareId.HasValue)
            query = query.Where(c => c.SoftwareId == softwareId.Value);

        return await query.SumAsync(c => c.TotalPrice);
    }

    public async Task<decimal> CalculateProjectedRevenueAsync(int? softwareId)
    {
        var query = _context.Contracts.AsQueryable();

        if (softwareId.HasValue)
            query = query.Where(c => c.SoftwareId == softwareId.Value);

        return await query.SumAsync(c => c.TotalPrice);
    }
}