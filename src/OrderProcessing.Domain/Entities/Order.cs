using OrderProcessing.Domain.Enums;

namespace OrderProcessing.Domain.Entities;

public class Order
{
    public Guid Id { get; private set; }
    public string CustomerId { get; private set; }
    public OrderStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private readonly List<OrderItem> _items = [];
    public IReadOnlyCollection<OrderItem> Items => _items;

    private Order() { }

    public Order(string customerId)
    {
        Id = Guid.NewGuid();
        CustomerId = customerId;
        Status = OrderStatus.PendingProcessing;
        CreatedAt = DateTime.UtcNow;
    }

    public void AddItem(string productId, int quantity)
    {
        _items.Add(new OrderItem(Id, productId, quantity));
    }
}
