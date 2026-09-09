using MediatorSite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace MediatorSite.Pages;

public class IndexModel : PageModel
{
    private readonly AppDbContext _context;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(
        AppDbContext context,
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration,
        ILogger<IndexModel> logger)
    {
        _context = context;
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
        _logger = logger;
    }

    [BindProperty]
    public Booking Booking { get; set; } = default!;

    [BindProperty]
    public string SelectedDate { get; set; } = string.Empty;

    [BindProperty]
    public string SelectedTime { get; set; } = string.Empty;

    public async Task OnGetAsync()
    {
        var takenBookings = await _context.Bookings
            .Where(b => b.BookingDate >= DateTime.Today)
            .Select(b => b.BookingDate.ToString("yyyy-MM-dd HH:mm"))
            .ToListAsync();

        ViewData["TakenSlots"] = JsonSerializer.Serialize(takenBookings);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (DateTime.TryParse($"{SelectedDate} {SelectedTime}", out DateTime fullDateTime))
        {
            Booking.BookingDate = fullDateTime;
        }

        ModelState.Remove("Booking.BookingDate");

        // Проверка занятости слота
        bool isSlotTaken = await _context.Bookings
            .AnyAsync(b => b.BookingDate == Booking.BookingDate);

        if (isSlotTaken)
        {
            ModelState.AddModelError(string.Empty, "Выбранное время уже занято. Пожалуйста, выберите другой слот.");
        }

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Форма записи не прошла валидацию.");
            await OnGetAsync();
            return Page();
        }

        _context.Bookings.Add(Booking);
        await _context.SaveChangesAsync();
       

        _logger.LogInformation("Новая заявка #{BookingId} успешно сохранена в БД.", Booking.BookingId);

        // Отправка в Telegram
        await SendTelegramNotificationAsync(Booking);

        TempData["SuccessMessage"] = "Ваша заявка успешно отправлена! Мы свяжемся с вами в ближайшее время.";

        return RedirectToPage("./Index");
    }

    private async Task SendTelegramNotificationAsync(Booking booking)
    {
        string botToken = _configuration["TelegramSettings:BotToken"]!;
        string chatId = _configuration["TelegramSettings:ChatId"]!;

        // 1. Очищаем номер до цифр для формирования URL
        string cleanPhone = new string((booking.Phone ?? "").Where(char.IsDigit).ToArray());
        if (cleanPhone.Length == 11 && cleanPhone.StartsWith("8"))
        {
            cleanPhone = "7" + cleanPhone.Substring(1);
        }

        // 2. Формируем безопасный текст с экранированием Markdown
        string name = SanitizeMarkdown(booking.CustomerName);
        string phone = SanitizeMarkdown(booking.Phone);
        string comments = SanitizeMarkdown(booking.SpecialRequests ?? "Не указан");

        string message = $" *Новая запись на консультацию!*\n\n" +
                         $" *Имя:* {name}\n" +
                         $" *Телефон:* {phone}\n" +
                         $" *Дата и время:* {booking.BookingDate:dd.MM.yyyy HH:mm}\n" +
                         $" *Комментарий:* {comments}";

        string url = $"https://api.telegram.org/bot{botToken}/sendMessage";

        // 3. Добавляем кнопки с роутингом только по HTTPS
        var buttons = new List<object>();
        if (cleanPhone.Length >= 7)
        {
            buttons.Add(new { text = " Написать в WhatsApp", url = $"https://wa.me/{cleanPhone}" });
        }

        var payload = new
        {
            chat_id = chatId,
            text = message,
            parse_mode = "Markdown",
            reply_markup = buttons.Any() ? new { inline_keyboard = new[] { buttons.ToArray() } } : null
        };

        try
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.PostAsJsonAsync(url, payload);

            if (!response.IsSuccessStatusCode)
            {
                string errorText = await response.Content.ReadAsStringAsync();
                _logger.LogError("Telegram API вернул ошибку: {StatusCode} - {ErrorText}", response.StatusCode, errorText);
            }
            else
            {
                _logger.LogInformation("Уведомление о заявке успешно отправлено в Telegram.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при отправке запроса к Telegram API.");
        }
    }

    private static string SanitizeMarkdown(string? text)
    {
        return (text ?? "")
            .Replace("*", "\\*")
            .Replace("_", "\\_")
            .Replace("`", "\\`");
    }
}