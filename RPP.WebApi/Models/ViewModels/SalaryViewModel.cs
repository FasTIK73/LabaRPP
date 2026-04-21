namespace RPP.WebApi.Models.ViewModels;

/// <summary>
/// Модель для отображения зарплаты работника
/// </summary>
public class SalaryViewModel
{
    public string WorkerId { get; set; } = string.Empty;
    public string WorkerName { get; set; } = string.Empty;
    public double Salary { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
}