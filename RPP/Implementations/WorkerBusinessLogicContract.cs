using Microsoft.Extensions.Logging;
using RPP.BusinessLogicsContracts;
using RPP.DataModels;
using RPP.Enums;
using RPP.Exceptions;
using RPP.Extensions;
using RPP.StoragesContracts;
using System.Text.RegularExpressions;

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
        _logger.LogInformation("GetAllWorkers called with onlyActive: {OnlyActive}", onlyActive);

        var workers = _workerStorage.GetList(onlyActive);
        if (workers == null)
            throw new NullListException();

        return workers;
    }

    public List<WorkerDataModel> GetWorkersByPost(WorkerPost post, bool onlyActive = true)
    {
        _logger.LogInformation("GetWorkersByPost called with post: {Post}, onlyActive: {OnlyActive}", post, onlyActive);

        if (post == WorkerPost.None)
            throw new ValidationException("Post cannot be None");

        var workers = _workerStorage.GetListByPost(post, onlyActive);
        if (workers == null)
            throw new NullListException();

        return workers;
    }

    public List<WorkerDataModel> GetWorkersByBirthDate(DateTime fromDate, DateTime toDate, bool onlyActive = true)
    {
        _logger.LogInformation("GetWorkersByBirthDate called from: {From} to: {To}, onlyActive: {OnlyActive}",
            fromDate, toDate, onlyActive);

        if (fromDate.IsDateNotOlder(toDate))
            throw new IncorrectDatesException(fromDate, toDate);

        var workers = _workerStorage.GetListByBirthDate(fromDate, toDate, onlyActive);
        if (workers == null)
            throw new NullListException();

        return workers;
    }

    public List<WorkerDataModel> GetWorkersByHireDate(DateTime fromDate, DateTime toDate, bool onlyActive = true)
    {
        _logger.LogInformation("GetWorkersByHireDate called from: {From} to: {To}, onlyActive: {OnlyActive}",
            fromDate, toDate, onlyActive);

        if (fromDate.IsDateNotOlder(toDate))
            throw new IncorrectDatesException(fromDate, toDate);

        var workers = _workerStorage.GetListByHireDate(fromDate, toDate, onlyActive);
        if (workers == null)
            throw new NullListException();

        return workers;
    }

    public WorkerDataModel GetWorkerByData(string data)
    {
        _logger.LogInformation("GetWorkerByData called with data: {Data}", data);

        if (data.IsEmpty())
            throw new ArgumentNullException(nameof(data));

        WorkerDataModel? worker = null;

        if (data.IsGuid())
        {
            worker = _workerStorage.GetElementById(data);
        }
        else if (Regex.IsMatch(data, @"^((8|\+7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?[\d\- ]{7,10}$"))
        {
            worker = _workerStorage.GetElementByPhone(data);
        }
        else if (data.Contains('@'))
        {
            worker = _workerStorage.GetElementByEmail(data);
        }

        return worker ?? throw new ElementNotFoundException(data);
    }

    public void InsertWorker(WorkerDataModel model)
    {
        _logger.LogInformation("InsertWorker called");

        ArgumentNullException.ThrowIfNull(model);

        model.Validate();

        try
        {
            _workerStorage.AddElement(model);
        }
        catch (Exception ex)
        {
            throw new StorageException("Error inserting worker", ex);
        }
    }

    public void UpdateWorker(WorkerDataModel model)
    {
        _logger.LogInformation("UpdateWorker called");

        ArgumentNullException.ThrowIfNull(model);

        model.Validate();

        try
        {
            _workerStorage.UpdateElement(model);
        }
        catch (Exception ex)
        {
            throw new StorageException("Error updating worker", ex);
        }
    }

    public void DeleteWorker(string id)
    {
        _logger.LogInformation("DeleteWorker called with id: {Id}", id);

        if (id.IsEmpty())
            throw new ArgumentNullException(nameof(id));

        if (!id.IsGuid())
            throw new ValidationException("Id is not a unique identifier");

        try
        {
            _workerStorage.DeleteElement(id);
        }
        catch (Exception ex)
        {
            throw new StorageException("Error deleting worker", ex);
        }
    }
}