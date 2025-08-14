using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RabbitMQ.Client;
using System.Text;

namespace AyazDuru.Samples.RabbitMQ.Publisher.Pages
{ 
    public class RabbitMQClientExampleModel : PageModel
    {
        private readonly ILogger<RabbitMQClientExampleModel> _logger;

        public RabbitMQClientExampleModel(ILogger<RabbitMQClientExampleModel> logger)
        {
            _logger = logger;
        }

        [BindProperty]
        public string Message { get; set; }

        public void OnGet()
        {
         
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // Model doðrulamasý baþarýsýz ise sayfayý tekrar göster
                return Page();
            }
            // RabbitMQ baðlantý ayarlarýný yapýlandýr
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "admin",
                Password = "admin",
                VirtualHost = "/"
            };

            // RabbitMQ sunucusuna asenkron baðlantý oluþtur
            using var connection = await factory.CreateConnectionAsync();
            // Kanal oluþtur
            using var channel = await connection.CreateChannelAsync();

            // 'messages' isminde bir exchange tanýmla (Fanout tipi)
            await channel.ExchangeDeclareAsync(exchange: "messages", type: ExchangeType.Fanout);

            // Mesajý UTF8 formatýnda byte dizisine dönüþtür
            var body = Encoding.UTF8.GetBytes(Message);
            // Mesajý exchange'e yayýnla
            await channel.BasicPublishAsync(exchange: "messages", routingKey: string.Empty, body: body);

            // Kanalý asenkron olarak kapat
            await channel.DisposeAsync();
            // Baþarýlý gönderim bilgisini TempData ile ilet
            TempData["Success"] = "Mesaj baþarýyla gönderildi!";
            // Sayfayý yeniden yönlendir
            return RedirectToPage();
        }
    }
}
