using System.ComponentModel.DataAnnotations;

namespace RPP.WebApi.Models.BindingModels;

/// <summary>
/// Модель для создания/обновления клиента
/// </summary>
public class ClientBindingModel
{
    /// <summary>
    /// ID клиента (при создании не указывается, при обновлении - обязателен)
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// ФИО клиента
    /// </summary>
    [Required(ErrorMessage = "Имя клиента обязательно")]
    [MinLength(2, ErrorMessage = "Имя должно содержать минимум 2 символа")]
    [MaxLength(200, ErrorMessage = "Имя не должно превышать 200 символов")]
    public string? Name { get; set; }

    /// <summary>
    /// Адрес клиента
    /// </summary>
    [Required(ErrorMessage = "Адрес обязателен")]
    [MaxLength(500, ErrorMessage = "Адрес не должен превышать 500 символов")]
    public string? Address { get; set; }

    /// <summary>
    /// Номер телефона
    /// </summary>
    [Required(ErrorMessage = "Номер телефона обязателен")]
    [Phone(ErrorMessage = "Неверный формат номера телефона")]
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Дата регистрации
    /// </summary>
    public DateTime RegistrationDate { get; set; } = DateTime.Now;
}