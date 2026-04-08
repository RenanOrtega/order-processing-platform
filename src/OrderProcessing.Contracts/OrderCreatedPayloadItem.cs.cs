namespace OrderProcessing.Contracts;

public class OrderCreatedPayloadItem(string productId, int quantity, decimal unitPrice)
{
    public string ProductId { get; set; } = productId;
    public int Quantity { get; set; } = quantity;
    public decimal UnitPrice { get; set; } = unitPrice;
}
