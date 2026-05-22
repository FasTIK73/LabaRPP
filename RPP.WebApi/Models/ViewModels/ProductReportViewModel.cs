namespace RPP.WebApi.Models.ViewModels;

/// <summary>
/// Модель для отображения отчета по продуктам
/// </summary>
public class ProductReportViewModel
{
    public string ManufacturerName { get; set; } = string.Empty;
    public List<ProductItemViewModel> Products { get; set; } = new();
    public int TotalCount { get; set; }
    public double TotalPrice { get; set; }
}

public class ProductItemViewModel
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string ProductType { get; set; } = string.Empty;
    public double Price { get; set; }
    public bool IsDeleted { get; set; }
}