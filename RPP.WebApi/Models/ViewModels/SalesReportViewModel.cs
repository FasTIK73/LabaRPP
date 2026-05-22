namespace RPP.WebApi.Models.ViewModels;

/// <summary>
/// Модель для отображения отчета по продажам
/// </summary>
public class SalesReportViewModel
{
    public DateTime ReportDate { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public List<SaleItemViewModel> Sales { get; set; } = new();
    public double TotalSum { get; set; }
    public double TotalDiscount { get; set; }
    public int TotalCount { get; set; }
}

public class SaleItemViewModel
{
    public DateTime SaleDate { get; set; }
    public double Sum { get; set; }
    public double Discount { get; set; }
    public string WorkerName { get; set; } = string.Empty;
    public string BuyerName { get; set; } = string.Empty;
    public List<ProductInSaleViewModel> Products { get; set; } = new();
}

public class ProductInSaleViewModel
{
    public string ProductName { get; set; } = string.Empty;
    public int Count { get; set; }
    public double Price { get; set; }
}