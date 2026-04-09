using RPP.Common.Enums;

namespace RPP.WebApi.Models.ViewModels;

/// <summary>
/// Модель для отображения данных помещения
/// </summary>
public class HomeViewModel
{
    public string Id { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public string ClientName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public double Area { get; set; }
    public HomeType Type { get; set; }
    public HomeStatus Status { get; set; }
}