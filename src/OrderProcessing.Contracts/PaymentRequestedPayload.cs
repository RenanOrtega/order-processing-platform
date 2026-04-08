namespace OrderProcessing.Contracts;

public class PaymentRequestedPayload(Guid orderId, decimal amount)
{
    public Guid OrderId { get; set; } = orderId;
    public decimal Amount { get; set; } = amount;
    public string PaymentMethod { get; set; } = "credit_card";
}
