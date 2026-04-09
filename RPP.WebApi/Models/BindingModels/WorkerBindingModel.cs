using System.ComponentModel.DataAnnotations;
using RPP.Common.Enums;

namespace RPP.WebApi.Models.BindingModels;

public class WorkerBindingModel
{
    public string? Id { get; set; }

    [Required(ErrorMessage = "ФИО обязательно")]
    [MinLength(2, ErrorMessage = "ФИО должно содержать минимум 2 символа")]
    public string? FullName { get; set; }

    [Required(ErrorMessage = "Номер телефона обязателен")]
    [Phone(ErrorMessage = "Неверный формат номера телефона")]
    public string? PhoneNumber { get; set; }

    [Required(ErrorMessage = "Email обязателен")]
    [EmailAddress(ErrorMessage = "Неверный формат email")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Должность обязательна")]
    public WorkerPost Post { get; set; }

    [Required(ErrorMessage = "Дата найма обязательна")]
    public DateTime HireDate { get; set; }

    [Required(ErrorMessage = "Дата рождения обязательна")]
    public DateTime BirthDate { get; set; }

    [Required(ErrorMessage = "Базовая ставка обязательна")]
    [Range(1, double.MaxValue, ErrorMessage = "Ставка должна быть больше 0")]
    public double BaseRate { get; set; }
}