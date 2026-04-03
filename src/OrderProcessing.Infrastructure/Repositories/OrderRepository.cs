using Microsoft.EntityFrameworkCore;
using OrderProcessing.Domain.Entities;
using OrderProcessing.Domain.Repositories.Interfaces;
using OrderProcessing.Infrastructure.Context;

namespace OrderProcessing.Infrastructure.Repositories;

public class OrderRepository(AppDbContext context) : IOrderRepository
{
    public async Task AddAsync(Order order, CancellationToken ct = default)
    {
        await context.Orders.AddAsync(order, ct);
    }

    public async Task<Order?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await context.Orders
             .Include(o => o.Items)
             .FirstOrDefaultAsync(o => o.Id.Equals(id), ct);
    }
}
