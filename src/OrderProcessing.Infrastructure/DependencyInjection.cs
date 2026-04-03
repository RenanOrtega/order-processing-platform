using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderProcessing.Domain.Repositories.Interfaces;
using OrderProcessing.Infrastructure.Context;
using OrderProcessing.Infrastructure.Persistance;
using OrderProcessing.Infrastructure.Repositories;

namespace OrderProcessing.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddDbContext(configuration)
            .AddRepositories();
    }

    private static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>((sp, options) =>
        {
            var connectionString = configuration.GetConnectionString("Postgres");

            options
                .UseNpgsql(connectionString)
                .UseSnakeCaseNamingConvention();
        });

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services
            .AddScoped<IOrderRepository, OrderRepository>()
            .AddScoped<IOutboxMessageRepository, OutboxMessageRepository>()
            .AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
