namespace RPP.WebApi.Models.ViewModels;

public class SalaryViewModel
{
    public string WorkerId { get; set; } = string.Empty;
    public string WorkerName { get; set; } = string.Empty;
    public double Salary { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
}