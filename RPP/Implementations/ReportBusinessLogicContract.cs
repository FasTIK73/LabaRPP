using Microsoft.Extensions.Logging;
using RPP.BusinessLogicsContracts;
using RPP.DataModels;
using RPP.Enums;
using RPP.Exceptions;
using RPP.Extensions;
using RPP.StoragesContracts;

namespace RPP.Implementations;

public class ReportBusinessLogicContract : IReportBusinessLogicContract
{
    private readonly ILogger<ReportBusinessLogicContract> _logger;
    private readonly IReportStorageContract _reportStorage;
    private readonly IWorkerStorageContract _workerStorage;
    private readonly IWorkTypeStorageContract _workTypeStorage;

    public ReportBusinessLogicContract(
        IReportStorageContract reportStorage,
        IWorkerStorageContract workerStorage,
        IWorkTypeStorageContract workTypeStorage,
        ILogger<ReportBusinessLogicContract> logger)
    {
        _reportStorage = reportStorage;
        _workerStorage = workerStorage;
        _workTypeStorage = workTypeStorage;
        _logger = logger;
    }

    public List<ReportDataModel> GetAllReports(DateTime? fromDate = null, DateTime? toDate = null)
    {
        _logger.LogInformation("GetAllReports called from: {From} to: {To}", fromDate, toDate);

        if (fromDate.HasValue && toDate.HasValue && fromDate.Value.IsDateNotOlder(toDate.Value))
            throw new IncorrectDatesException(fromDate.Value, toDate.Value);

        var reports = _reportStorage.GetList(fromDate, toDate);
        if (reports == null)
            throw new NullListException();

        return reports;
    }

    public List<ReportDataModel> GetReportsByHome(string homeId)
    {
        _logger.LogInformation("GetReportsByHome called with homeId: {HomeId}", homeId);

        if (homeId.IsEmpty())
            throw new ArgumentNullException(nameof(homeId));

        if (!homeId.IsGuid())
            throw new ValidationException("HomeId is not a unique identifier");

        var reports = _reportStorage.GetListByHome(homeId);
        if (reports == null)
            throw new NullListException();

        return reports;
    }

    public List<ReportDataModel> GetReportsByWorker(string workerId)
    {
        _logger.LogInformation("GetReportsByWorker called with workerId: {WorkerId}", workerId);

        if (workerId.IsEmpty())
            throw new ArgumentNullException(nameof(workerId));

        if (!workerId.IsGuid())
            throw new ValidationException("WorkerId is not a unique identifier");

        var reports = _reportStorage.GetListByWorker(workerId);
        if (reports == null)
            throw new NullListException();

        return reports;
    }

    public List<ReportDataModel> GetReportsByWorkType(string workTypeId)
    {
        _logger.LogInformation("GetReportsByWorkType called with workTypeId: {WorkTypeId}", workTypeId);

        if (workTypeId.IsEmpty())
            throw new ArgumentNullException(nameof(workTypeId));

        if (!workTypeId.IsGuid())
            throw new ValidationException("WorkTypeId is not a unique identifier");

        var reports = _reportStorage.GetListByWorkType(workTypeId);
        if (reports == null)
            throw new NullListException();

        return reports;
    }

    public List<ReportDataModel> GetReportsByTool(string toolId)
    {
        _logger.LogInformation("GetReportsByTool called with toolId: {ToolId}", toolId);

        if (toolId.IsEmpty())
            throw new ArgumentNullException(nameof(toolId));

        if (!toolId.IsGuid())
            throw new ValidationException("ToolId is not a unique identifier");

        var reports = _reportStorage.GetListByTool(toolId);
        if (reports == null)
            throw new NullListException();

        return reports;
    }

    public ReportDataModel GetReportById(string id)
    {
        _logger.LogInformation("GetReportById called with id: {Id}", id);

        if (id.IsEmpty())
            throw new ArgumentNullException(nameof(id));

        if (!id.IsGuid())
            throw new ValidationException("Id is not a unique identifier");

        var report = _reportStorage.GetElementById(id);
        return report ?? throw new ElementNotFoundException(id);
    }

    public void InsertReport(ReportDataModel model)
    {
        _logger.LogInformation("InsertReport called");

        ArgumentNullException.ThrowIfNull(model);

        model.Validate();

        try
        {
            _reportStorage.AddElement(model);
        }
        catch (Exception ex)
        {
            throw new StorageException("Error inserting report", ex);
        }
    }

    public void UpdateReport(ReportDataModel model)
    {
        _logger.LogInformation("UpdateReport called");

        ArgumentNullException.ThrowIfNull(model);

        model.Validate();

        try
        {
            _reportStorage.UpdateElement(model);
        }
        catch (Exception ex)
        {
            throw new StorageException("Error updating report", ex);
        }
    }

    public void CancelReport(string id)
    {
        _logger.LogInformation("CancelReport called with id: {Id}", id);

        if (id.IsEmpty())
            throw new ArgumentNullException(nameof(id));

        if (!id.IsGuid())
            throw new ValidationException("Id is not a unique identifier");

        try
        {
            _reportStorage.DeleteElement(id);
        }
        catch (Exception ex)
        {
            throw new StorageException("Error canceling report", ex);
        }
    }

    public double CalculateWorkerSalary(string workerId, DateTime fromDate, DateTime toDate)
    {
        _logger.LogInformation("CalculateWorkerSalary called for worker: {WorkerId} from: {From} to: {To}",
            workerId, fromDate, toDate);

        if (workerId.IsEmpty())
            throw new ArgumentNullException(nameof(workerId));

        if (!workerId.IsGuid())
            throw new ValidationException("WorkerId is not a unique identifier");

        if (fromDate.IsDateNotOlder(toDate))
            throw new IncorrectDatesException(fromDate, toDate);

        var worker = _workerStorage.GetElementById(workerId);
        if (worker == null)
            throw new ElementNotFoundException(workerId);

        var reports = _reportStorage.GetListByWorker(workerId);
        if (reports == null)
            return 0;

        // Фильтруем отчеты по дате
        var filteredReports = reports.Where(r => r.WorkDate >= fromDate && r.WorkDate <= toDate).ToList();

        // Расчет зарплаты: базовый оклад + процент от выполненных работ
        double totalSalary = worker.BaseRate;
        double totalWorkCost = filteredReports.Sum(r => r.TotalCost);

        // Процент от работ зависит от должности
        double bonusPercent = worker.Post switch
        {
            WorkerPost.Master => 0.3,    // 30%
            WorkerPost.Handyman => 0.2,   // 20%
            WorkerPost.Foreman => 0.4,    // 40%
            WorkerPost.Assistant => 0.1,  // 10%
            _ => 0
        };

        totalSalary += totalWorkCost * bonusPercent;

        return totalSalary;
    }
}