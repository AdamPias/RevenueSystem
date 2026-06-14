using Microsoft.EntityFrameworkCore;
using RevenueSystem.Application.Repositories;

namespace RevenueSystem.Infrastructure.Repositories;

public class DiscountRepository : IDiscountRepository
{
    private readonly AppDbContext _context;
    public DiscountRepository(AppDbContext context) => _context = context;

    public async Task<decimal> GetHighestActiveDiscountAsync(DateTime targetDate)
    {
        var activeDiscounts = await _context.Discounts
            .Where(d => targetDate >= d.DateFrom && targetDate <= d.DateTo)
            .OrderByDescending(d => d.ValuePercentage)
            .FirstOrDefaultAsync();

        return activeDiscounts?.ValuePercentage ?? 0m;
    }
}