using AyazDuru.Samples.RabbitMQ.Core;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;

namespace AyazDuru.Samples.RabbitMQ.Publisher.Pages
{
    public class MassTransitExampleModel : PageModel
    {
        private readonly ILogger<MassTransitExampleModel> _logger; // Logger servisi, loglama işleri için kullanılır
        private readonly IBus _bus; // Mesaj göndermek için kullanılan bus nesnesi

        public MassTransitExampleModel(ILogger<MassTransitExampleModel> logger, IBus bus)
        {
            _logger = logger;
            _bus = bus;
        }

        [BindProperty]
        public string Message { get; set; } // Gönderilecek mesajın içeriği

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _bus.Publish(new Message { Text = Message }); // Mesajı yayınla
            TempData["Success"] = "Mesaj başarıyla gönderildi!"; // Başarı mesajını ayarla
            return RedirectToPage(); // Sayfayı yeniden yönlendir
        }
    }
}
