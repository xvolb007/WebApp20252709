using Application.Interaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Messaging
{
    public class RabbitMqHostedService : BackgroundService
    {
        private readonly IRabbitMqConsumer _consumer;
        private readonly ILogger<RabbitMqHostedService> _logger;

        public RabbitMqHostedService(IRabbitMqConsumer consumer, ILogger<RabbitMqHostedService> logger)
        {
            _consumer = consumer;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("RabbitMQ Hosted Service started");
            await _consumer.Consume("calc-queue", stoppingToken);
            _logger.LogInformation("RabbitMQ Hosted Service stopped");
        }
    }
}
