using System.ComponentModel.DataAnnotations;

namespace MediatorSite.Models;

public class Booking
{
    public int BookingId { get; set; }

    [Required(ErrorMessage = "Введите ваше имя")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Имя должно быть от 2 до 50 символов")]
    public string CustomerName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Укажите желаемую дату и время")]
    [DataType(DataType.DateTime)]
    public DateTime BookingDate { get; set; } = DateTime.Now.AddDays(1);

    [Required(ErrorMessage = "Введите номер телефона")]
    [Phone(ErrorMessage = "Некорректный формат номера телефона")]
    [StringLength(20, MinimumLength = 7, ErrorMessage = "Номер телефона должен содержать от 7 до 20 символов")]
    public string Phone { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Комментарий не должен превышать 500 символов")]
    public string? SpecialRequests { get; set; }

    // Статус заявки
    public string Status { get; set; } = "Новая";
}