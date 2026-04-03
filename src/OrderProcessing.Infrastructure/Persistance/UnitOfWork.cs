using OrderProcessing.Domain.Repositories.Interfaces;
using OrderProcessing.Infrastructure.Context;

namespace OrderProcessing.Infrastructure.Persistance;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        return context.SaveChangesAsync(ct);
    }
}
