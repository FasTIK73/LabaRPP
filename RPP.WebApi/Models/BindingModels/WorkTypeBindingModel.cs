using System.ComponentModel.DataAnnotations;
using RPP.Common.Enums;

namespace RPP.WebApi.Models.BindingModels;

/// <summary>
/// Модель для создания/обновления типа работы
/// </summary>
public class WorkTypeBindingModel
{
    public string? Id { get; set; }

    [Required(ErrorMessage = "Название работы обязательно")]
    [MinLength(2, ErrorMessage = "Название должно содержать минимум 2 символа")]
    [MaxLength(200, ErrorMessage = "Название не должно превышать 200 символов")]
    public string? WorkName { get; set; }

    [Required(ErrorMessage = "Единица измерения обязательна")]
    public MeasurementUnit Unit { get; set; }

    [Required(ErrorMessage = "Цена за единицу обязательна")]
    [Range(1, double.MaxValue, ErrorMessage = "Цена должна быть больше 0")]
    public double PricePerUnit { get; set; }
}