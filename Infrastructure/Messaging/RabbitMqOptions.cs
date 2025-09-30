

namespace Infrastructure.Messaging
{
    public class RabbitMqOptions
    {
        public string HostName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public List<RabbitMqQueueOptions> Queues { get; set; } = new();
    }
    public class RabbitMqQueueOptions
    {
        public string Name { get; set; } = string.Empty;
        public bool Durable { get; set; } = true;
        public bool Exclusive { get; set; } = false;
        public bool AutoDelete { get; set; } = false;
        public Dictionary<string, object>? Arguments { get; set; }
    }

}
