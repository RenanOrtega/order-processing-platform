namespace OrderProcessing.Domain.Enums;

public enum OrderStatus
{
    PendingProcessing = 0,
    PaymentPending = 1,
    PaymentApproved = 2,
    PaymentFailed = 3,
    InventoryReserved = 4,
    InventoryFailed = 5,
    ReadyForShipment = 6,
    Completed = 7,
    Cancelled = 8,
    Expired = 9,
    DeadLettered = 10
}
