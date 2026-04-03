using System.Text.Json;
using MediatR;
using OrderProcessing.Application.CreateOrder.Commands;
using OrderProcessing.Domain.Constants.OutboxMessage;
using OrderProcessing.Domain.Entities;
using OrderProcessing.Domain.Models.OutboxMessage;
using OrderProcessing.Domain.Repositories.Interfaces;

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
            order.AddItem(item.ProductId, item.Quantity);
        }

        var outbox = new OutboxMessage(EventTypes.OrderCreated, JsonSerializer.Serialize(new PayloadJson(order.Id, order.CustomerId, EventTypes.OrderCreated)));

        await _orderRepository.AddAsync(order, ct);
        await _outboxMessageRepository.AddAsync(outbox, ct);
        await _unitOfWork.SaveChangesAsync(ct);
    }
}
