using Amazon.SQS;
using OrderProcessing.Infrastructure;
using OutboxPublisher.Worker;
using OutboxPublisher.Worker.Configurations;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.Configure<AwsSettings>(builder.Configuration.GetSection("AwsSettings"));
builder.Services.AddAWSService<IAmazonSQS>();

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
