using System.ComponentModel.DataAnnotations;
using RPP.Common.Enums;

namespace RPP.WebApi.Models.BindingModels;

/// <summary>
/// Модель для создания/обновления помещения
/// </summary>
public class HomeBindingModel
{
    public string? Id { get; set; }

    [Required(ErrorMessage = "ID клиента обязателен")]
    public string? ClientId { get; set; }

    [Required(ErrorMessage = "Адрес обязателен")]
    [MaxLength(500, ErrorMessage = "Адрес не должен превышать 500 символов")]
    public string? Address { get; set; }

    [Required(ErrorMessage = "Площадь обязательна")]
    [Range(1, double.MaxValue, ErrorMessage = "Площадь должна быть больше 0")]
    public double Area { get; set; }

    [Required(ErrorMessage = "Тип помещения обязателен")]
    public HomeType Type { get; set; }

    [Required(ErrorMessage = "Статус обязателен")]
    public HomeStatus Status { get; set; }
}