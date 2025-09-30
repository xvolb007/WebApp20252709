using Application.Interaces;
using Domain.QueueTasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Infrastructure.Messaging
{
    public class RabbitMqConsumer : IRabbitMqConsumer
    {
        private readonly RabbitMqOptions _config;
        private readonly ILogger<RabbitMqConsumer> _logger;

        public RabbitMqConsumer(IOptions<RabbitMqOptions> options, ILogger<RabbitMqConsumer> logger)
        {
            _config = options.Value;
            _logger = logger;
        }

        public async Task Consume(string queue, CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory
            {
                HostName = _config.HostName,
                UserName = _config.UserName,
                Password = _config.Password
            };

            await using var connection = await factory.CreateConnectionAsync();
            await using var channel = await connection.CreateChannelAsync();
            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += async (_, ea) =>
            {
                try
                {
                    var json = Encoding.UTF8.GetString(ea.Body.ToArray());

                    string? taskType = null;
                    if (ea.BasicProperties.Headers != null && ea.BasicProperties.Headers.TryGetValue("TaskType", out var raw))
                    {
                        if (raw is byte[] bytes)
                            taskType = Encoding.UTF8.GetString(bytes);
                        else
                            taskType = raw?.ToString();
                    }


                    switch (taskType)
                    {
                        case "Calculation":
                            var calcTask = JsonSerializer.Deserialize<CalculationTask>(json);
                            if (calcTask != null)
                            {
                                _logger.LogInformation(
                                    " [x] Received CalculationTask from {queue}: Key={Key}, Computed={Computed}, Input={Input}, Previous={Previous}",
                                    queue,
                                    calcTask.Key,
                                    calcTask.ComputedValue,
                                    calcTask.InputValue,
                                    calcTask.PreviousValue
                                );
                            }
                            break;

                        default:
                            _logger.LogWarning("Unknown task type: {taskType}", taskType);
                            break;
                    }

                    await channel.BasicAckAsync(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while processing message from {queue}", queue);
                    await channel.BasicNackAsync(ea.DeliveryTag, false, requeue: true);
                }
            };

            await channel.BasicConsumeAsync(queue, autoAck: false, consumer, stoppingToken);
            _logger.LogInformation(" [*] Waiting for messages in {queue}...", queue);
            try
            {
                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
            catch (OperationCanceledException) { }
            
        }
    }

}
