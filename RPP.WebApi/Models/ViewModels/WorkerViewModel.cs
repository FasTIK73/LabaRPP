using RPP.Common.Enums;

namespace RPP.WebApi.Models.ViewModels;

public class WorkerViewModel
{
    public string Id { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public WorkerPost Post { get; set; }
    public DateTime HireDate { get; set; }
    public DateTime BirthDate { get; set; }
    public double BaseRate { get; set; }
    public bool IsDeleted { get; set; }
}