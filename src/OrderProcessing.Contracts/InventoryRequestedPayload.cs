namespace OrderProcessing.Contracts;

public class InventoryRequestedPayload(Guid orderId, IList<InventoryRequestedPayloadItem> items)
{
    public Guid OrderId { get; set; } = orderId;
    public IList<InventoryRequestedPayloadItem> Items { get; set; } = items;
}
