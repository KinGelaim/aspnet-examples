using RabbitMQ.Client;
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
    durable: true, // Сохранять на диск, чтобы очередь не была потеряна при перезапуске брокера
    exclusive: false, // Может использоваться другими соединениями
    autoDelete: false, // Не удалять, когда последний потребитель отключается
    arguments: null);

for (var i = 0; i < 10; i++)
{
    var message = $"{DateTime.Now} - {Guid.CreateVersion7()}";
    var body = Encoding.UTF8.GetBytes(message);

    // Публикация сообщения
    await channel.BasicPublishAsync(
        exchange: string.Empty, // Exchange по умолчанию
        routingKey: "messages",
        mandatory: true, // Сбой, если сообщение не может быть перенаправлено
        basicProperties: new BasicProperties { Persistent = true }, // Сообщение будет сохранено на диске
        body: body);

    Console.WriteLine($"Sent: {message}");

    await Task.Delay(3000);
}

Console.ReadKey();