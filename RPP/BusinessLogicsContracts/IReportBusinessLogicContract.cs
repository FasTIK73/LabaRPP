using RPP.DataModels;

namespace RPP.BusinessLogicsContracts;

public interface IReportBusinessLogicContract
{
    Task<List<ReportDataModel>> GetAllReportsAsync(DateTime? fromDate = null, DateTime? toDate = null);
    Task<List<ReportDataModel>> GetReportsByHomeAsync(string homeId);
    Task<List<ReportDataModel>> GetReportsByWorkerAsync(string workerId);
    Task<List<ReportDataModel>> GetReportsByWorkTypeAsync(string workTypeId);
    Task<List<ReportDataModel>> GetReportsByToolAsync(string toolId);
    Task<ReportDataModel?> GetReportByIdAsync(string id);
    Task InsertReportAsync(ReportDataModel model);
    Task UpdateReportAsync(ReportDataModel model);
    Task CancelReportAsync(string id);
    Task<double> CalculateWorkerSalaryAsync(string workerId, DateTime fromDate, DateTime toDate);

    // НОВЫЕ МЕТОДЫ ДЛЯ 6 ЛАБЫ
    Task<List<(string ManufacturerName, List<ProductDataModel> Products)>> GetProductsGroupedByManufacturerAsync(bool onlyActive = true, string? manufacturerId = null);
    Task<List<SaleDataModel>> GetSalesForPeriodAsync(DateTime fromDate, DateTime toDate, string? workerId = null);
    Task<List<(WorkerDataModel Worker, double Salary)>> GetSalariesForPeriodAsync(DateTime fromDate, DateTime toDate);
}