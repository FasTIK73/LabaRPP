using OfficeOpenXml;
using OfficeOpenXml.Style;
using RPP.BusinessLogicsContracts;
using RPP.WebApi.Models.BindingModels;
using RPP.WebApi.Models.ViewModels;
using System.Drawing;

namespace RPP.WebApi.Services;

public class ReportService
{
    private readonly IReportBusinessLogicContract _reportLogic;

    public ReportService(IReportBusinessLogicContract reportLogic)
    {
        _reportLogic = reportLogic;
    }

    public async Task<byte[]> GenerateProductReportAsync(ProductReportBindingModel model)
    {
        var groupedProducts = await _reportLogic.GetProductsGroupedByManufacturerAsync(model.OnlyActive, model.ManufacturerId);

        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Продукты");

        worksheet.Cells[1, 1].Value = "ОТЧЕТ ПО ПРОДУКТАМ";
        worksheet.Cells[1, 1, 1, 5].Merge = true;
        worksheet.Cells[1, 1].Style.Font.Size = 16;
        worksheet.Cells[1, 1].Style.Font.Bold = true;
        worksheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        worksheet.Cells[3, 1].Value = "Производитель";
        worksheet.Cells[3, 2].Value = "Название продукта";
        worksheet.Cells[3, 3].Value = "Тип";
        worksheet.Cells[3, 4].Value = "Цена";
        worksheet.Cells[3, 5].Value = "Статус";

        worksheet.Cells[3, 1, 3, 5].Style.Font.Bold = true;
        worksheet.Cells[3, 1, 3, 5].Style.Fill.PatternType = ExcelFillStyle.Solid;
        worksheet.Cells[3, 1, 3, 5].Style.Fill.BackgroundColor.SetColor(Color.LightGray);

        int row = 4;
        int totalCount = 0;
        double totalPrice = 0;

        foreach (var group in groupedProducts)
        {
            var firstRow = row;
            foreach (var product in group.Products)
            {
                worksheet.Cells[row, 1].Value = group.ManufacturerName;
                worksheet.Cells[row, 2].Value = product.ProductName;
                worksheet.Cells[row, 3].Value = product.ProductType.ToString();
                worksheet.Cells[row, 4].Value = product.Price;
                worksheet.Cells[row, 4].Style.Numberformat.Format = "#,##0.00";
                worksheet.Cells[row, 5].Value = product.IsDeleted ? "Удален" : "Активен";
                totalCount++;
                totalPrice += product.Price;
                row++;
            }
            worksheet.Cells[firstRow, 1, row - 1, 1].Merge = true;
        }

        worksheet.Cells[row, 2].Value = "ИТОГО:";
        worksheet.Cells[row, 2].Style.Font.Bold = true;
        worksheet.Cells[row, 4].Value = totalPrice;
        worksheet.Cells[row, 4].Style.Numberformat.Format = "#,##0.00";
        worksheet.Cells[row, 5].Value = totalCount;

        worksheet.Cells.AutoFitColumns();
        return package.GetAsByteArray();
    }

    public async Task<byte[]> GenerateSalesReportAsync(SalesReportBindingModel model)
    {
        var sales = await _reportLogic.GetSalesForPeriodAsync(model.FromDate, model.ToDate, model.WorkerId);

        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Продажи");

        worksheet.Cells[1, 1].Value = "ОТЧЕТ ПО ПРОДАЖАМ";
        worksheet.Cells[1, 1, 1, 5].Merge = true;
        worksheet.Cells[1, 1].Style.Font.Size = 16;
        worksheet.Cells[1, 1].Style.Font.Bold = true;
        worksheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        worksheet.Cells[2, 1].Value = $"Период: {model.FromDate:dd.MM.yyyy} - {model.ToDate:dd.MM.yyyy}";
        worksheet.Cells[2, 1, 2, 5].Merge = true;

        worksheet.Cells[4, 1].Value = "Дата";
        worksheet.Cells[4, 2].Value = "Сотрудник";
        worksheet.Cells[4, 3].Value = "Покупатель";
        worksheet.Cells[4, 4].Value = "Сумма";
        worksheet.Cells[4, 5].Value = "Скидка";

        worksheet.Cells[4, 1, 4, 5].Style.Font.Bold = true;
        worksheet.Cells[4, 1, 4, 5].Style.Fill.PatternType = ExcelFillStyle.Solid;
        worksheet.Cells[4, 1, 4, 5].Style.Fill.BackgroundColor.SetColor(Color.LightGray);

        int row = 5;
        double totalSum = 0;
        double totalDiscount = 0;

        foreach (var sale in sales)
        {
            worksheet.Cells[row, 1].Value = sale.SaleDate.ToString("dd.MM.yyyy");
            worksheet.Cells[row, 2].Value = sale.WorkerFIO;
            worksheet.Cells[row, 3].Value = string.IsNullOrEmpty(sale.BuyerFIO) ? "Не указан" : sale.BuyerFIO;
            worksheet.Cells[row, 4].Value = sale.Sum;
            worksheet.Cells[row, 4].Style.Numberformat.Format = "#,##0.00";
            worksheet.Cells[row, 5].Value = sale.Discount;
            worksheet.Cells[row, 5].Style.Numberformat.Format = "#,##0.00";
            totalSum += sale.Sum;
            totalDiscount += sale.Discount;
            row++;
        }

        worksheet.Cells[row, 3].Value = "ИТОГО:";
        worksheet.Cells[row, 3].Style.Font.Bold = true;
        worksheet.Cells[row, 4].Value = totalSum;
        worksheet.Cells[row, 4].Style.Numberformat.Format = "#,##0.00";
        worksheet.Cells[row, 5].Value = totalDiscount;
        worksheet.Cells[row, 5].Style.Numberformat.Format = "#,##0.00";

        worksheet.Cells.AutoFitColumns();
        return package.GetAsByteArray();
    }

    public async Task<byte[]> GenerateSalaryReportAsync(SalaryReportBindingModel model)
    {
        var salaries = await _reportLogic.GetSalariesForPeriodAsync(model.FromDate, model.ToDate);

        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Зарплаты");

        worksheet.Cells[1, 1].Value = "ОТЧЕТ ПО ЗАРПЛАТАМ";
        worksheet.Cells[1, 1, 1, 3].Merge = true;
        worksheet.Cells[1, 1].Style.Font.Size = 16;
        worksheet.Cells[1, 1].Style.Font.Bold = true;
        worksheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        worksheet.Cells[2, 1].Value = $"Период: {model.FromDate:dd.MM.yyyy} - {model.ToDate:dd.MM.yyyy}";
        worksheet.Cells[2, 1, 2, 3].Merge = true;

        worksheet.Cells[4, 1].Value = "ФИО сотрудника";
        worksheet.Cells[4, 2].Value = "Должность";
        worksheet.Cells[4, 3].Value = "Начислено";

        worksheet.Cells[4, 1, 4, 3].Style.Font.Bold = true;
        worksheet.Cells[4, 1, 4, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
        worksheet.Cells[4, 1, 4, 3].Style.Fill.BackgroundColor.SetColor(Color.LightGray);

        int row = 5;
        double totalSalary = 0;

        foreach (var salary in salaries)
        {
            worksheet.Cells[row, 1].Value = salary.Worker.FullName;
            worksheet.Cells[row, 2].Value = salary.Worker.Post.ToString();
            worksheet.Cells[row, 3].Value = salary.Salary;
            worksheet.Cells[row, 3].Style.Numberformat.Format = "#,##0.00";
            totalSalary += salary.Salary;
            row++;
        }

        worksheet.Cells[row, 1].Value = "ИТОГО:";
        worksheet.Cells[row, 1].Style.Font.Bold = true;
        worksheet.Cells[row, 3].Value = totalSalary;
        worksheet.Cells[row, 3].Style.Numberformat.Format = "#,##0.00";

        worksheet.Cells.AutoFitColumns();
        return package.GetAsByteArray();
    }
}