namespace OrderProcessing.Contracts;

public class OrderCreatedPayload(Guid id, string customerId, string eventType, decimal amount, IList<OrderCreatedPayloadItem> items)
{
    public Guid Id { get; set; } = id;
    public string CustomerId { get; set; } = customerId;
    public decimal Amount { get; set; } = amount;
    public IList<OrderCreatedPayloadItem> Items { get; set; } = items; 
    public string EventType { get; set; } = eventType;
}
