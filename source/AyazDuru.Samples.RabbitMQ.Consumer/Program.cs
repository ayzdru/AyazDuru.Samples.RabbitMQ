using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using static MassTransit.Logging.OperationName;

namespace AyazDuru.Samples.RabbitMQ.Consumer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Uygulama için varsayılan host yapılandırmasını oluşturur
            var builder = Host.CreateDefaultBuilder(args);

            builder.ConfigureServices(services =>
            {
                // RabbitMQ Client dinleyici arkaplan servisini ekler
                services.AddHostedService<RabbitMQClientConsumerService>();
                // MassTransit yapılandırması başlatılır
                services.AddMassTransit(x =>
                {
                    // Mesaj tüketici sınıfını ekler
                    x.AddConsumer<MassTransitMessageConsumer>();
                    // RabbitMQ bağlantı ve endpoint ayarları yapılır
                    x.UsingRabbitMq((context, cfg) =>
                    {
                        // RabbitMQ sunucu bağlantı bilgileri
                        cfg.Host("localhost", "/", h => {
                            h.Username("admin"); // Kullanıcı adı
                            h.Password("admin"); // Şifre
                        });

                        // Mesajların alınacağı endpoint tanımlanır
                        cfg.ReceiveEndpoint("masstransit-messages", e =>
                        {
                            // Tüketici sınıfı endpoint'e bağlanır
                            e.Consumer<MassTransitMessageConsumer>(context);
                        });
                    });
                });

            });

            // Host nesnesi oluşturulur ve uygulama başlatılır
            var host = builder.Build();
            host.Run();
        }
    }
}
