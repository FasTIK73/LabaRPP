namespace RPP.WebApi.Models.ViewModels;

/// <summary>
/// Модель для отображения данных клиента
/// </summary>
public class ClientViewModel
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public DateTime RegistrationDate { get; set; }
}