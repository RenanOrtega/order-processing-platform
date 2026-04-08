namespace OrderProcessing.Contracts;

public class InventoryRequestedPayloadItem(string productId, int quantity)
{
    public string Sku { get; set; } = productId;
    public int Quantity { get; set; } = quantity;
}
