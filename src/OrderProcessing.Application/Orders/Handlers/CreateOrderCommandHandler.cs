using MediatR;
using OrderProcessing.Application.Orders.Commands;
using OrderProcessing.Contracts;
using OrderProcessing.Domain.Constants.OutboxMessage;
using OrderProcessing.Domain.Entities;
using OrderProcessing.Domain.Repositories.Interfaces;
using System.Text.Json;

namespace OrderProcessing.Application.Orders.Handlers;

public class CreateOrderCommandHandler(
    IOrderRepository orderRepository,
    IOutboxMessageRepository outboxMessageRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateOrderCommand>
{
    private readonly IOrderRepository _orderRepository = orderRepository;
    private readonly IOutboxMessageRepository _outboxMessageRepository = outboxMessageRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(CreateOrderCommand request, CancellationToken ct)
    {
        var order = new Order(request.CustomerId);
        foreach (var item in request.Items)
        {
            order.AddItem(item.ProductId, item.Quantity, item.UnitPrice);
        }

        var outbox = new OutboxMessage(
            EventTypes.OrderCreated,
            JsonSerializer.Serialize(
                new OrderCreatedPayload(
                    order.Id,
                    order.CustomerId,
                    EventTypes.OrderCreated,
                    order.TotalAmount,
                    [.. order.Items.Select(i => new OrderCreatedPayloadItem(i.ProductId, i.Quantity, i.UnitPrice))]
                )
            )
        );

        await _orderRepository.AddAsync(order, ct);
        await _outboxMessageRepository.AddAsync(outbox, ct);
        await _unitOfWork.SaveChangesAsync(ct);
    }
}
