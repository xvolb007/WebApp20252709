

namespace Domain.QueueTasks

{
    public interface IRabbitMqTask
    {
        string TaskType { get; }
    }
}
