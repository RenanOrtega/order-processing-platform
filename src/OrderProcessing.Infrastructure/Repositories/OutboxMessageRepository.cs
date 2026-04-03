using Microsoft.EntityFrameworkCore;
using OrderProcessing.Domain.Entities;
using OrderProcessing.Domain.Repositories.Interfaces;
using OrderProcessing.Infrastructure.Context;

namespace OrderProcessing.Infrastructure.Repositories;

public class OutboxMessageRepository(AppDbContext context) : IOutboxMessageRepository
{
    public async Task AddAsync(OutboxMessage outboxMessage, CancellationToken ct = default)
    {
        await context.OutboxMessages.AddAsync(outboxMessage, ct);
    }

    public async Task<OutboxMessage?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await context.OutboxMessages
             .FirstOrDefaultAsync(o => o.Id.Equals(id), ct);
    }
}
