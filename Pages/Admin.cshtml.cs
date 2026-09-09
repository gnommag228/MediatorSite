using MediatorSite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace MediatorSite.Pages;

public class AdminModel : PageModel
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public AdminModel(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public List<Booking> Bookings { get; set; } = new();
    public bool IsAuthenticated { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        IsAuthenticated = HttpContext.Session.GetString("IsAdmin") == "true";

        if (IsAuthenticated)
        {
            // Сортируем заявки: сначала самые свежие
            Bookings = await _context.Bookings.OrderByDescending(b => b.BookingId).ToListAsync();
        }

        return Page();
    }

    public IActionResult OnPost(string password)
    {
        // Читаем пароль из файла appsettings.json
        string? adminPassword = _configuration["AdminSettings:Password"];

        if (!string.IsNullOrEmpty(adminPassword) && password == adminPassword)
        {
            HttpContext.Session.SetString("IsAdmin", "true");
            return RedirectToPage();
        }

        ModelState.AddModelError(string.Empty, "Неверный пароль!");
        return Page();
    }

    public IActionResult OnPostLogout()
    {
        HttpContext.Session.Remove("IsAdmin");
        return RedirectToPage();
    }

    // Удаление заявки
    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true") return RedirectToPage();

        var booking = await _context.Bookings.FindAsync(id);
        if (booking != null)
        {
            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage();
    }

    // Изменение статуса
    public async Task<IActionResult> OnPostUpdateStatusAsync(int id, string status)
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true") return RedirectToPage();

        var booking = await _context.Bookings.FindAsync(id);
        if (booking != null)
        {
            booking.Status = status;
            await _context.SaveChangesAsync();
        }

        return RedirectToPage();
    }
}