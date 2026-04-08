using MediatR;

namespace OrderProcessing.Application.Orders.Commands;

public class CreateOrderCommand : IRequest
{
    public string CustomerId { get; set; } = string.Empty;
    public IList<CreateOrderItems> Items { get; set; } = [];
}

public class CreateOrderItems
{
    public string ProductId { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}