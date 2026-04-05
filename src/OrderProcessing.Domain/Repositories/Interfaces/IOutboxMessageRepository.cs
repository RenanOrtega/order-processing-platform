using OrderProcessing.Domain.Entities;

namespace OrderProcessing.Domain.Repositories.Interfaces;

public interface IOutboxMessageRepository
{
    Task AddAsync(OutboxMessage outboxMessage, CancellationToken ct = default);
    Task<OutboxMessage?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<OutboxMessage>> GetPendingMessagesAsync(CancellationToken ct = default);
}
