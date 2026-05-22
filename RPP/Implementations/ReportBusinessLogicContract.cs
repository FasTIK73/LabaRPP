using Microsoft.Extensions.Logging;
using RPP.BusinessLogicsContracts;
using RPP.Common.Exceptions;
using RPP.Common.Extensions;
using RPP.Common.Enums;
using RPP.DataModels;
using RPP.StoragesContracts;

namespace RPP.Implementations;

public class ReportBusinessLogicContract : IReportBusinessLogicContract
{
    private readonly IReportStorageContract _reportStorage;
    private readonly IWorkerStorageContract _workerStorage;
    private readonly IWorkTypeStorageContract _workTypeStorage;
    private readonly IProductStorageContract _productStorage;
    private readonly IManufacturerStorageContract _manufacturerStorage;
    private readonly IPostStorageContract _postStorage;
    private readonly IBuyerStorageContract _buyerStorage;
    private readonly ILogger _logger;

    public ReportBusinessLogicContract(
        IReportStorageContract reportStorage,
        IWorkerStorageContract workerStorage,
        IWorkTypeStorageContract workTypeStorage,
        IProductStorageContract productStorage,
        IManufacturerStorageContract manufacturerStorage,
        IPostStorageContract postStorage,
        IBuyerStorageContract buyerStorage,
        ILogger<ReportBusinessLogicContract> logger)
    {
        _reportStorage = reportStorage;
        _workerStorage = workerStorage;
        _workTypeStorage = workTypeStorage;
        _productStorage = productStorage;
        _manufacturerStorage = manufacturerStorage;
        _postStorage = postStorage;
        _buyerStorage = buyerStorage;
        _logger = logger;
    }

    public async Task<List<ReportDataModel>> GetAllReportsAsync(DateTime? fromDate = null, DateTime? toDate = null)
    {
        return await Task.Run(() => _reportStorage.GetList(fromDate, toDate) ?? throw new NullListException());
    }

    public async Task<List<ReportDataModel>> GetReportsByHomeAsync(string homeId)
    {
        if (string.IsNullOrEmpty(homeId))
            throw new ArgumentNullException(nameof(homeId));
        return await Task.Run(() => _reportStorage.GetListByHome(homeId) ?? throw new NullListException());
    }

    public async Task<List<ReportDataModel>> GetReportsByWorkerAsync(string workerId)
    {
        if (string.IsNullOrEmpty(workerId))
            throw new ArgumentNullException(nameof(workerId));
        return await Task.Run(() => _reportStorage.GetListByWorker(workerId) ?? throw new NullListException());
    }

    public async Task<List<ReportDataModel>> GetReportsByWorkTypeAsync(string workTypeId)
    {
        if (string.IsNullOrEmpty(workTypeId))
            throw new ArgumentNullException(nameof(workTypeId));
        return await Task.Run(() => _reportStorage.GetListByWorkType(workTypeId) ?? throw new NullListException());
    }

    public async Task<List<ReportDataModel>> GetReportsByToolAsync(string toolId)
    {
        if (string.IsNullOrEmpty(toolId))
            throw new ArgumentNullException(nameof(toolId));
        return await Task.Run(() => _reportStorage.GetListByTool(toolId) ?? throw new NullListException());
    }

    public async Task<ReportDataModel?> GetReportByIdAsync(string id)
    {
        if (string.IsNullOrEmpty(id))
            throw new ArgumentNullException(nameof(id));
        return await Task.Run(() => _reportStorage.GetElementById(id));
    }

    public async Task InsertReportAsync(ReportDataModel model)
    {
        ArgumentNullException.ThrowIfNull(model);
        model.Validate();
        await Task.Run(() => _reportStorage.AddElement(model));
    }

    public async Task UpdateReportAsync(ReportDataModel model)
    {
        ArgumentNullException.ThrowIfNull(model);
        model.Validate();
        await Task.Run(() => _reportStorage.UpdateElement(model));
    }

    public async Task CancelReportAsync(string id)
    {
        if (string.IsNullOrEmpty(id))
            throw new ArgumentNullException(nameof(id));
        await Task.Run(() => _reportStorage.DeleteElement(id));
    }

    public async Task<double> CalculateWorkerSalaryAsync(string workerId, DateTime fromDate, DateTime toDate)
    {
        if (string.IsNullOrEmpty(workerId))
            throw new ArgumentNullException(nameof(workerId));

        return await Task.Run(() => CalculateWorkerSalary(workerId, fromDate, toDate));
    }

    private double CalculateWorkerSalary(string workerId, DateTime fromDate, DateTime toDate)
    {
        var worker = _workerStorage.GetElementById(workerId);
        if (worker == null)
            throw new ElementNotFoundException(workerId);

        var reports = _reportStorage.GetListByWorker(workerId);
        var filteredReports = reports?.Where(r => r.WorkDate >= fromDate && r.WorkDate <= toDate).ToList() ?? new List<ReportDataModel>();

        double totalSalary = worker.BaseRate;
        double totalWorkCost = filteredReports.Sum(r => r.TotalCost);

        double bonusPercent = worker.Post switch
        {
            WorkerPost.Master => 0.3,
            WorkerPost.Handyman => 0.2,
            WorkerPost.Foreman => 0.4,
            WorkerPost.Assistant => 0.1,
            _ => 0
        };

        return totalSalary + totalWorkCost * bonusPercent;
    }

    public async Task<List<(string ManufacturerName, List<ProductDataModel> Products)>> GetProductsGroupedByManufacturerAsync(bool onlyActive = true, string? manufacturerId = null)
    {
        var result = await Task.Run(() =>
        {
            var products = _productStorage.GetList(onlyActive, manufacturerId) ?? new List<ProductDataModel>();
            if (!products.Any())
                return new List<(string, List<ProductDataModel>)>();

            var manufacturers = _manufacturerStorage.GetList() ?? new List<ManufacturerDataModel>();
            var grouped = products.GroupBy(p => p.ManufacturerId)
                .Select(g => (
                    ManufacturerName: manufacturers.FirstOrDefault(m => m.Id == g.Key)?.ManufacturerName ?? "Неизвестный производитель",
                    Products: g.ToList()
                ))
                .OrderBy(x => x.ManufacturerName)
                .ToList();

            return grouped;
        });

        return result;
    }

    public async Task<List<SaleDataModel>> GetSalesForPeriodAsync(DateTime fromDate, DateTime toDate, string? workerId = null)
    {
        if (fromDate >= toDate)
            throw new IncorrectDatesException(fromDate, toDate);

        return await Task.Run(() =>
        {
            var reports = _reportStorage.GetList(fromDate, toDate, workerId) ?? new List<ReportDataModel>();
            var sales = new List<SaleDataModel>();

            foreach (var report in reports)
            {
                var worker = _workerStorage.GetElementById(report.WorkerId);
                var buyer = report.BuyerId != null ? _buyerStorage.GetElementById(report.BuyerId) : null;

                var sale = new SaleDataModel(
                    report.Id, report.WorkerId, report.BuyerId,
                    report.TotalCost, DiscountType.None, 0, false, null,
                    worker?.FullName ?? string.Empty,
                    buyer?.Name ?? string.Empty
                );
                sales.Add(sale);
            }
            return sales;
        });
    }

    public async Task<List<(WorkerDataModel Worker, double Salary)>> GetSalariesForPeriodAsync(DateTime fromDate, DateTime toDate)
    {
        if (fromDate >= toDate)
            throw new IncorrectDatesException(fromDate, toDate);

        return await Task.Run(() =>
        {
            var workers = _workerStorage.GetList(true) ?? new List<WorkerDataModel>();
            var salaries = new List<(WorkerDataModel, double)>();

            foreach (var worker in workers)
            {
                var salary = CalculateWorkerSalary(worker.Id, fromDate, toDate);
                salaries.Add((worker, salary));
            }
            return salaries.OrderByDescending(x => x.Item2).ToList();
        });
    }
}