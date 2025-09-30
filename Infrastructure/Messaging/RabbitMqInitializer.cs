using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Infrastructure.Messaging
{
    public class RabbitMqInitializer
    {
        private readonly RabbitMqOptions _options;

        public RabbitMqInitializer(IOptions<RabbitMqOptions> options)
        {
            _options = options.Value;
        }

        public async Task InitializeAsync()
        {
            var factory = new ConnectionFactory
            {
                HostName = _options.HostName,
                UserName = _options.UserName,
                Password = _options.Password
            };

            await using var connection = await factory.CreateConnectionAsync();
            await using var channel = await connection.CreateChannelAsync();

            foreach (var queue in _options.Queues)
            {
                await channel.QueueDeclareAsync(
                    queue: queue.Name,
                    durable: queue.Durable,
                    exclusive: queue.Exclusive,
                    autoDelete: queue.AutoDelete,
                    arguments: queue.Arguments
                );
            }
        }
    }
}
