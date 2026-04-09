using RPP.Common.Enums;

namespace RPP.WebApi.Models.ViewModels;

/// <summary>
/// Модель для отображения данных отчета
/// </summary>
public class ReportViewModel
{
    public string Id { get; set; } = string.Empty;
    public string HomeId { get; set; } = string.Empty;
    public string HomeAddress { get; set; } = string.Empty;
    public string WorkTypeId { get; set; } = string.Empty;
    public string WorkTypeName { get; set; } = string.Empty;
    public string WorkerId { get; set; } = string.Empty;
    public string WorkerName { get; set; } = string.Empty;
    public string ToolId { get; set; } = string.Empty;
    public string ToolName { get; set; } = string.Empty;
    public DateTime WorkDate { get; set; }
    public double WorkVolume { get; set; }
    public ReportStatus Status { get; set; }
    public double TotalCost { get; set; }
}