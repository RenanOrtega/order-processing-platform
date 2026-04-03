namespace OrderProcessing.Domain.Models.OutboxMessage;

public class PayloadJson
{
    public PayloadJson(Guid id, string customerId, string eventType)
    {
        Id = id;
        CustomerId = customerId;
        EventType = eventType;
    }

    public Guid Id { get; set; }
    public string CustomerId { get; set; }
    public string EventType { get; set; }
}
