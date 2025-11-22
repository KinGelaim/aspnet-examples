using Confluent.Kafka;

var bootstrapServers = "localhost:9092";
var groupId = "my-consumer-group";
var topicName = "my-topic";

var consumerConfig = new ConsumerConfig
{
    BootstrapServers = bootstrapServers,
    GroupId = groupId,
    AutoOffsetReset = AutoOffsetReset.Earliest // Начинает использовать с самого начала, если смещение не сохранено
};

var cts = new CancellationTokenSource();
Console.CancelKeyPress += (_, e) =>
{
    e.Cancel = true; // Прерывание процесса из терминала
    cts.Cancel();
};

using var consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
consumer.Subscribe(topicName);

Console.WriteLine($"Consuming messages from topic: {topicName}");

try
{
    while (!cts.IsCancellationRequested)
    {
        try
        {
            var consumeResult = consumer.Consume(cts.Token);
            Console.WriteLine($"Consumed message '{consumeResult.Message.Value}' at: {consumeResult.TopicPartitionOffset}");
        }
        catch (ConsumeException e)
        {
            Console.WriteLine($"Error occurred: {e.Error.Reason}");
        }
    }
}
catch (OperationCanceledException)
{
    Console.WriteLine("Consumer was cancelled");
}
finally
{
    consumer.Close(); // Зафиксируйте смещения и аккуратно покиньте группу
}

Console.ReadKey();