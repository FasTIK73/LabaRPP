namespace RPP.WebApi.Models.ViewModels;

/// <summary>
/// Модель для отображения данных инструмента
/// </summary>
public class ToolViewModel
{
    public string Id { get; set; } = string.Empty;
    public string ToolName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsAvailable { get; set; }
    public string? PreviousToolName { get; set; }
}