using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory()
{
    HostName = "localhost",
    Port = 5672,
    UserName = "user",
    Password = "bitnami"
};
using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

// Проверяем наличие очереди (если очереди нет, то создаём её)
await channel.QueueDeclareAsync(
    queue: "messages",
    durable: true, // Должен соответствовать настройкам производителя
    exclusive: false, // Может использоваться другими соединениями
    autoDelete: false, // Не удалять, когда последний потребитель отключается
    arguments: null);

Console.WriteLine("Waiting for messages...");

// Определяем потребителя и начинаем слушать
var consumer = new AsyncEventingBasicConsumer(channel);
consumer.ReceivedAsync += async (sender, eventArgs) =>
{
    byte[] body = eventArgs.Body.ToArray();
    string message = Encoding.UTF8.GetString(body);

    Console.WriteLine($"Received: {message}");

    // Подтверждаем получение сообщения
    await ((AsyncEventingBasicConsumer)sender)
        .Channel.BasicAckAsync(eventArgs.DeliveryTag, multiple: false);
};
await channel.BasicConsumeAsync("messages", autoAck: false, consumer);

Console.ReadKey();