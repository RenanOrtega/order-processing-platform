using OrderProcessing.Domain.Entities;

namespace OrderProcessing.Domain.Repositories.Interfaces;

public interface IOrderRepository
{
    Task AddAsync(Order order, CancellationToken ct = default);
    Task<Order?> GetByIdAsync(Guid id, CancellationToken ct = default);
}
