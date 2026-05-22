namespace RPP.WebApi.Models.BindingModels;

/// <summary>
/// Модель для запроса отчета по продажам
/// </summary>
public class SalesReportBindingModel
{
    /// <summary>
    /// Формат отчета: Excel, Pdf, Word
    /// </summary>
    public string Format { get; set; } = "Excel";

    /// <summary>
    /// Дата начала периода
    /// </summary>
    public DateTime FromDate { get; set; }

    /// <summary>
    /// Дата окончания периода
    /// </summary>
    public DateTime ToDate { get; set; }

    /// <summary>
    /// Фильтр по работнику (опционально)
    /// </summary>
    public string? WorkerId { get; set; }
}