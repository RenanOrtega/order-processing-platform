namespace OrderProcessing.Domain.Entities;

public class OrderItem
{
    public Guid Id { get; private set; }
    public Guid OrderId { get; private set; }
    public string ProductId { get; private set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; private set; }
    public decimal Total => UnitPrice * Quantity;

    private OrderItem() { }

    public OrderItem(Guid orderId, string productId, int quantity, decimal unitPrice)
    {
        Id = Guid.NewGuid();
        OrderId = orderId;
        ProductId = productId;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }
}
