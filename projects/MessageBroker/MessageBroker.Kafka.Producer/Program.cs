using Confluent.Kafka;

var bootstrapServers = "localhost:9092";
var topicName = "my-topic";

var producerConfig = new ProducerConfig { BootstrapServers = bootstrapServers };

using var producer = new ProducerBuilder<Null, string>(producerConfig).Build();

Console.WriteLine($"Producing message to topic: {topicName}");

for (var i = 0; i < 10; i++)
{
    var message = $"{DateTime.Now} - {Guid.CreateVersion7()}";
    Console.WriteLine($"Sent: {message}");

    producer.Produce(
        topicName,
        new Message<Null, string> { Value = message },
        deliveryReport =>
        {
            if (deliveryReport.Error.Code != ErrorCode.NoError)
            {
                Console.WriteLine($"Failed to deliver message: {deliveryReport.Error.Reason}");
            }
            else
            {
                Console.WriteLine($"Produced message to: {deliveryReport.TopicPartitionOffset}");
            }
        });

    await Task.Delay(3000);
}

producer.Flush(TimeSpan.FromSeconds(10)); // Убедиться, что все сообщения отправлены

Console.ReadKey();