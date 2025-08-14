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
                // RabbitMQ konfig�rasyonu yap�l�yor
                x.UsingRabbitMq((context, cfg) =>
                {
                    // RabbitMQ sunucusuna ba�lant� ayarlar�
                    cfg.Host("localhost", "/", h =>
                    {
                        h.Username("admin"); // Kullan�c� ad�
                        h.Password("admin"); // �ifre
                    });
                    // Mesajlar�n al�naca�� endpoint tan�mlan�yor
                    cfg.ReceiveEndpoint("masstransit-messages", e =>
                    {
                        // Burada mesaj i�leyicileri eklenebilir
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
