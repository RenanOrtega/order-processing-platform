using Amazon.SQS;
using Microsoft.Extensions.Options;
using OrderProcessing.Domain.Repositories.Interfaces;
using OutboxPublisher.Worker.Configurations;

namespace OutboxPublisher.Worker;

public class Worker(
    ILogger<Worker> logger,
    IServiceScopeFactory serviceScopeFactory,
    IAmazonSQS amazonSqs,
    IOptions<AwsSettings> options) : BackgroundService
{
    private readonly ILogger<Worker> _logger = logger;
    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;
    private readonly IAmazonSQS _amazonSqs = amazonSqs;
    private readonly AwsSettings _awsSettings = options.Value;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var outboxMessageRepository = scope.ServiceProvider.GetRequiredService<IOutboxMessageRepository>();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            _logger.LogInformation("Getting pending messages...");
            var pendingMessages = await outboxMessageRepository.GetPendingMessagesAsync(stoppingToken);
            _logger.LogInformation("Found {Count} messages.", pendingMessages.Count());

            if (pendingMessages.Any())
            {
                foreach (var message in pendingMessages)
                {
                    _logger.LogInformation("Sending to SQS queue: {QueueUrl} - Id: {Id}", _awsSettings.OrderCreatedQueueUrl, message.Id);
                    await _amazonSqs.SendMessageAsync(_awsSettings.OrderCreatedQueueUrl, message.Payload, stoppingToken);
                    _logger.LogInformation("Marking message as processed: {Id}", message.Id);
                    message.MarkAsProcessed();
                }

                _logger.LogInformation("Saving changes in outbox messages...");
                await unitOfWork.SaveChangesAsync(stoppingToken);
            }

            _logger.LogInformation("Starting delay 5 seconds...");
            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
    }
}
