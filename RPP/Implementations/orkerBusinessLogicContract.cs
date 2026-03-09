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

public class WorkerBusinessLogicContract : IWorkerBusinessLogicContract
{
    private readonly ILogger<WorkerBusinessLogicContract> _logger;
    private readonly IWorkerStorageContract _workerStorage;

    public WorkerBusinessLogicContract(
        IWorkerStorageContract workerStorage,
        ILogger<WorkerBusinessLogicContract> logger)
    {
        _workerStorage = workerStorage;
        _logger = logger;
    }

    public List<WorkerDataModel> GetAllWorkers(bool onlyActive = true)
    {
        throw new NotImplementedException();
    }

    public List<WorkerDataModel> GetWorkersByPost(WorkerPost post, bool onlyActive = true)
    {
        throw new NotImplementedException();
    }

    public List<WorkerDataModel> GetWorkersByBirthDate(DateTime fromDate, DateTime toDate, bool onlyActive = true)
    {
        throw new NotImplementedException();
    }

    public List<WorkerDataModel> GetWorkersByHireDate(DateTime fromDate, DateTime toDate, bool onlyActive = true)
    {
        throw new NotImplementedException();
    }

    public WorkerDataModel GetWorkerByData(string data)
    {
        throw new NotImplementedException();
    }

    public void InsertWorker(WorkerDataModel model)
    {
        throw new NotImplementedException();
    }

    public void UpdateWorker(WorkerDataModel model)
    {
        throw new NotImplementedException();
    }

    public void DeleteWorker(string id)
    {
        throw new NotImplementedException();
    }
}