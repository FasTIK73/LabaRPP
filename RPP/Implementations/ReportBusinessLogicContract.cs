using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;
using RPP.BusinessLogicsContracts;
using RPP.DataModels;
using RPP.Enums;
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
        throw new NotImplementedException();
    }

    public List<ReportDataModel> GetReportsByHome(string homeId)
    {
        throw new NotImplementedException();
    }

    public List<ReportDataModel> GetReportsByWorker(string workerId)
    {
        throw new NotImplementedException();
    }

    public List<ReportDataModel> GetReportsByWorkType(string workTypeId)
    {
        throw new NotImplementedException();
    }

    public List<ReportDataModel> GetReportsByTool(string toolId)
    {
        throw new NotImplementedException();
    }

    public ReportDataModel GetReportById(string id)
    {
        throw new NotImplementedException();
    }

    public void InsertReport(ReportDataModel model)
    {
        throw new NotImplementedException();
    }

    public void UpdateReport(ReportDataModel model)
    {
        throw new NotImplementedException();
    }

    public void CancelReport(string id)
    {
        throw new NotImplementedException();
    }

    public double CalculateWorkerSalary(string workerId, DateTime fromDate, DateTime toDate)
    {
        throw new NotImplementedException();
    }
}