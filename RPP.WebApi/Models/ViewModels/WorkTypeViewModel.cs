using RPP.Common.Enums;

namespace RPP.WebApi.Models.ViewModels;

/// <summary>
/// Модель для отображения данных типа работы
/// </summary>
public class WorkTypeViewModel
{
    public string Id { get; set; } = string.Empty;
    public string WorkName { get; set; } = string.Empty;
    public MeasurementUnit Unit { get; set; }
    public double PricePerUnit { get; set; }
    public double? PreviousPrice { get; set; }
    public DateTime? PriceChangeDate { get; set; }
}