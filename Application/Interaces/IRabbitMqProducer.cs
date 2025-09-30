
using Domain.QueueTasks;

namespace Application.Interaces
{
    public interface IRabbitMqProducer
    {
        Task Publish<T>(string queue, T task) where T : IRabbitMqTask;
    }
}
