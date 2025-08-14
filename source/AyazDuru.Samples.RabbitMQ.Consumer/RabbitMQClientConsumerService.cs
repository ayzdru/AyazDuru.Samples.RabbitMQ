using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AyazDuru.Samples.RabbitMQ.Consumer
{
    public class RabbitMQClientConsumerService : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // RabbitMQ bağlantı ayarları oluşturuluyor.
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "admin",
                Password = "admin",
                VirtualHost = "/"
            };
            // RabbitMQ sunucusuna bağlantı kuruluyor.
            using var connection = await factory.CreateConnectionAsync();
            // Bağlantı üzerinden bir kanal oluşturuluyor.
            using var channel = await connection.CreateChannelAsync();

            // "messages" isminde bir exchange (değişim noktası) oluşturuluyor. Tipi Fanout.
            await channel.ExchangeDeclareAsync(exchange: "messages",
                type: ExchangeType.Fanout);

            // Geçici bir kuyruk oluşturuluyor.
            QueueDeclareOk queueDeclareResult = await channel.QueueDeclareAsync();
            string queueName = queueDeclareResult.QueueName;
            // Kuyruk, "messages" exchange'ine bağlanıyor.
            await channel.QueueBindAsync(queue: queueName, exchange: "messages", routingKey: string.Empty);


            // Tüketici (consumer) nesnesi oluşturuluyor.
            var consumer = new AsyncEventingBasicConsumer(channel);
            // Mesaj alındığında çalışacak olay tanımlanıyor.
            consumer.ReceivedAsync += (model, ea) =>
            {
                // Mesajın içeriği byte dizisinden okunuyor.
                byte[] body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                // Mesaj ekrana yazdırılıyor.
                Console.WriteLine($" [RabbitMQ.Client] {message}");
                return Task.CompletedTask;
            };

            // Tüketici, kuyruğa bağlanıyor ve mesajları otomatik olarak onaylıyor.
            await channel.BasicConsumeAsync(queueName, autoAck: true, consumer: consumer);

            // Servis durdurulana kadar bekleniyor.
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
    }
}
