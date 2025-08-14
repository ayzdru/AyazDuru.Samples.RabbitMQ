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
                // Model do�rulamas� ba�ar�s�z ise sayfay� tekrar g�ster
                return Page();
            }
            // RabbitMQ ba�lant� ayarlar�n� yap�land�r
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "admin",
                Password = "admin",
                VirtualHost = "/"
            };

            // RabbitMQ sunucusuna asenkron ba�lant� olu�tur
            using var connection = await factory.CreateConnectionAsync();
            // Kanal olu�tur
            using var channel = await connection.CreateChannelAsync();

            // 'messages' isminde bir exchange tan�mla (Fanout tipi)
            await channel.ExchangeDeclareAsync(exchange: "messages", type: ExchangeType.Fanout);

            // Mesaj� UTF8 format�nda byte dizisine d�n��t�r
            var body = Encoding.UTF8.GetBytes(Message);
            // Mesaj� exchange'e yay�nla
            await channel.BasicPublishAsync(exchange: "messages", routingKey: string.Empty, body: body);

            // Kanal� asenkron olarak kapat
            await channel.DisposeAsync();
            // Ba�ar�l� g�nderim bilgisini TempData ile ilet
            TempData["Success"] = "Mesaj ba�ar�yla g�nderildi!";
            // Sayfay� yeniden y�nlendir
            return RedirectToPage();
        }
    }
}
