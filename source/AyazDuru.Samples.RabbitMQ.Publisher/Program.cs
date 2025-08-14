using AyazDuru.Samples.RabbitMQ.Publisher.Pages;
using MassTransit;

namespace AyazDuru.Samples.RabbitMQ.Publisher
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);            
            builder.Services.AddRazorPages();

            // Servis ekleniyor (MassTransit)
            builder.Services.AddMassTransit(x =>
            {
                // RabbitMQ konfigürasyonu yapýlýyor
                x.UsingRabbitMq((context, cfg) =>
                {
                    // RabbitMQ sunucusuna baðlantý ayarlarý
                    cfg.Host("localhost", "/", h =>
                    {
                        h.Username("admin"); // Kullanýcý adý
                        h.Password("admin"); // Þifre
                    });
                    // Mesajlarýn alýnacaðý endpoint tanýmlanýyor
                    cfg.ReceiveEndpoint("masstransit-messages", e =>
                    {
                        // Burada mesaj iþleyicileri eklenebilir
                    });
                });
            });
            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error"); 
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets(); 
            app.MapRazorPages()
               .WithStaticAssets(); 

            app.Run();
        }
    }
}
