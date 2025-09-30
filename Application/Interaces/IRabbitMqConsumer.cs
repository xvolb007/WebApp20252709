
namespace Application.Interaces
{
    public interface IRabbitMqConsumer
    {
        Task Consume(string queue, CancellationToken stoppingToken);
    }
}
