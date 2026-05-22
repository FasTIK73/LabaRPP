namespace RPP.WebApi.Models.BindingModels;

/// <summary>
/// Модель для запроса отчета по продуктам
/// </summary>
public class ProductReportBindingModel
{
    /// <summary>
    /// Формат отчета: Excel, Pdf, Word
    /// </summary>
    public string Format { get; set; } = "Excel";

    /// <summary>
    /// Только активные продукты
    /// </summary>
    public bool OnlyActive { get; set; } = true;

    /// <summary>
    /// Фильтр по производителю (опционально)
    /// </summary>
    public string? ManufacturerId { get; set; }
}