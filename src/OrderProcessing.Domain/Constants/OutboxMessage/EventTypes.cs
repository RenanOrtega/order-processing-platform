namespace OrderProcessing.Domain.Constants.OutboxMessage;

public static class EventTypes
{
    public const string OrderCreated = "order.created";
    public const string PaymentRequested = "payment.requested";
    public const string PaymentApproved = "payment.approved";
    public const string PaymentFailed = "payment.failed";
    public const string InventoryReserved = "inventory.reserved";
    public const string InventoryFailed = "inventory.failed";
    public const string NotificationRequested = "notification.requested";
    public const string OrderCompleted = "order.completed";
}
