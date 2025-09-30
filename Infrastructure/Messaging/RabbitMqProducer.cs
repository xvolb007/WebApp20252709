using Application.Interaces;
using Domain.QueueTasks;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
namespace Infrastructure.Messaging
{
    public class RabbitMqProducer : IRabbitMqProducer
    {
        private readonly ConnectionFactory _factory;

        public RabbitMqProducer(IOptions<RabbitMqOptions> options)
        {
            var config = options.Value;
            _factory = new ConnectionFactory
            {
                HostName = config.HostName,
                UserName = config.UserName,
                Password = config.Password
            };
        }
        public async Task Publish<T>(string queue, T task) where T : IRabbitMqTask
        {
            await using var connection = await _factory.CreateConnectionAsync();
            await using var channel = await connection.CreateChannelAsync();

            var json = JsonSerializer.Serialize(task);
            var body = Encoding.UTF8.GetBytes(json);

            var props = new BasicProperties
            {
                ContentType = "application/json",
                DeliveryMode = DeliveryModes.Persistent,
                Headers = new Dictionary<string, object?>
                {
                    { "TaskType", task.TaskType }
                }
            };

            await channel.BasicPublishAsync(
                exchange: "",
                routingKey: queue,
                mandatory: false,
                basicProperties: props,
                body: body);
        }
    }
}
