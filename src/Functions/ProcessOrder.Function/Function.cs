using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using Amazon.SQS;
using Microsoft.Extensions.DependencyInjection;
using OrderProcessing.Contracts;
using System.Text.Json;
using static Amazon.Lambda.SQSEvents.SQSBatchResponse;


// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace ProcessOrder.Function
{
    public class Function
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IAmazonSQS _sqs;
        /// <summary>
        /// Default constructor. This constructor is used by Lambda to construct the instance. When invoked in a Lambda environment
        /// the AWS credentials will come from the IAM role associated with the function and the AWS region will be set to the
        /// region the Lambda function is executed in.
        /// </summary>
        public Function()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            _serviceProvider = serviceCollection.BuildServiceProvider();
            _sqs = _serviceProvider.GetRequiredService<IAmazonSQS>();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddAWSService<IAmazonSQS>();
        }

        /// <summary>
        /// This method is called for every Lambda invocation. This method takes in an SQS event object and can be used 
        /// to respond to SQS messages.
        /// </summary>
        /// <param name="evnt">The event for the Lambda function handler to process.</param>
        /// <param name="context">The ILambdaContext that provides methods for logging and describing the Lambda environment.</param>
        /// <returns></returns>
        public async Task<SQSBatchResponse> FunctionHandler(SQSEvent evnt, ILambdaContext context)
        {
            var tasks = evnt.Records.Select(async message =>
            {
                try
                {
                    await ProcessMessageAsync(message, context);
                    return null;
                }
                catch (Exception ex)
                {
                    context.Logger.LogError($"Error processing message {message.MessageId}: {ex.Message}");
                    return message.MessageId;
                }
            });

            var results = await Task.WhenAll(tasks);

            return new SQSBatchResponse
            {
                BatchItemFailures = [.. results
                    .Where(id => id != null)
                    .Select(id => new BatchItemFailure
                    {
                        ItemIdentifier = id,
                    })
                ],
            };
        }

        private async Task ProcessMessageAsync(SQSEvent.SQSMessage message, ILambdaContext context)
        {
            var orderCreatedPayload = JsonSerializer.Deserialize<OrderCreatedPayload>(message.Body)
                ?? throw new ArgumentNullException(nameof(message), "Invalid messageBody.");

            await SendToPaymentAsync(orderCreatedPayload);
            await SendToInventoryAsync(orderCreatedPayload);

            //await _sqs.SendMessageAsync("notification url", message.Body);

            context.Logger.LogInformation($"Processed message {message.Body}");
        }

        private async Task SendToInventoryAsync(OrderCreatedPayload orderCreatedPayload)
        {
            var inventoryRequestedPayload = new InventoryRequestedPayload(orderCreatedPayload.Id, [.. orderCreatedPayload.Items.Select(i => new InventoryRequestedPayloadItem(i.ProductId, i.Quantity))]);
            await _sqs.SendMessageAsync("inventory url", JsonSerializer.Serialize(inventoryRequestedPayload));
        }

        private async Task SendToPaymentAsync(OrderCreatedPayload orderCreatedPayload)
        {
            var paymentRequestedPayload = new PaymentRequestedPayload(orderCreatedPayload.Id, orderCreatedPayload.Amount);
            await _sqs.SendMessageAsync("payment url", JsonSerializer.Serialize(paymentRequestedPayload));
        }
    }
}