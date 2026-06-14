using Microsoft.EntityFrameworkCore;
using RevenueSystem.Application.Repositories;
using RevenueSystem.Domain.Entities;

namespace RevenueSystem.Infrastructure.Repositories;

public class SoftwareRepository : ISoftwareRepository
{
    private readonly AppDbContext _context;
    public SoftwareRepository(AppDbContext context) => _context = context;

    public async Task<Software?> GetSoftwareByIdAsync(int id)
    {
        return await _context.Softwares.FirstOrDefaultAsync(s => s.Id == id);
    }
}