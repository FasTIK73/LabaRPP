namespace RPP.WebApi.Models.ViewModels;

/// <summary>
/// Модель для отображения отчета по зарплатам
/// </summary>
public class SalaryReportViewModel
{
    public DateTime ReportDate { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public List<SalaryItemViewModel> Salaries { get; set; } = new();
    public double TotalSalary { get; set; }
    public double AverageSalary { get; set; }
    public int WorkersCount { get; set; }
}

public class SalaryItemViewModel
{
    public string WorkerId { get; set; } = string.Empty;
    public string WorkerName { get; set; } = string.Empty;
    public string PostName { get; set; } = string.Empty;
    public double Salary { get; set; }
}