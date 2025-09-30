namespace Domain.QueueTasks
{
    public class CalculationTask : IRabbitMqTask
    {
        public string TaskType => "Calculation";
        public static string QueueName => "calc-queue";

        public int Key { get; set; }
        public decimal ComputedValue { get; set; }
        public decimal InputValue { get; set; }
        public decimal PreviousValue { get; set; }
    }
}
