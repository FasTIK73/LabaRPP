using Microsoft.Extensions.Logging;
using RPP.BusinessLogicsContracts;
using RPP.DataModels;
using RPP.Enums;
using RPP.Exceptions;
using RPP.Extensions;
using RPP.StoragesContracts;

namespace RPP.Implementations;

public class WorkTypeBusinessLogicContract : IWorkTypeBusinessLogicContract
{
    private readonly ILogger<WorkTypeBusinessLogicContract> _logger;
    private readonly IWorkTypeStorageContract _workTypeStorage;

    public WorkTypeBusinessLogicContract(
        IWorkTypeStorageContract workTypeStorage,
        ILogger<WorkTypeBusinessLogicContract> logger)
    {
        _workTypeStorage = workTypeStorage;
        _logger = logger;
    }

    public List<WorkTypeDataModel> GetAllWorkTypes()
    {
        _logger.LogInformation("GetAllWorkTypes called");

        var workTypes = _workTypeStorage.GetList();
        if (workTypes == null)
            throw new NullListException();

        return workTypes;
    }

    public WorkTypeDataModel GetWorkTypeByData(string data)
    {
        _logger.LogInformation("GetWorkTypeByData called with data: {Data}", data);

        if (data.IsEmpty())
            throw new ArgumentNullException(nameof(data));

        WorkTypeDataModel? workType = null;

        if (data.IsGuid())
        {
            workType = _workTypeStorage.GetElementById(data);
        }
        else
        {
            workType = _workTypeStorage.GetElementByName(data);
        }

        return workType ?? throw new ElementNotFoundException(data);
    }

    public List<WorkTypeDataModel> GetPriceHistory(string id)
    {
        _logger.LogInformation("GetPriceHistory called with id: {Id}", id);

        if (id.IsEmpty())
            throw new ArgumentNullException(nameof(id));

        if (!id.IsGuid())
            throw new ValidationException("Id is not a unique identifier");

        var history = _workTypeStorage.GetPriceHistory(id);
        if (history == null)
            throw new NullListException();

        return history;
    }

    public void InsertWorkType(WorkTypeDataModel model)
    {
        _logger.LogInformation("InsertWorkType called");

        ArgumentNullException.ThrowIfNull(model);

        model.Validate();

        try
        {
            _workTypeStorage.AddElement(model);
        }
        catch (Exception ex)
        {
            throw new StorageException("Error inserting work type", ex);
        }
    }

    public void UpdateWorkType(WorkTypeDataModel model)
    {
        _logger.LogInformation("UpdateWorkType called");

        ArgumentNullException.ThrowIfNull(model);

        model.Validate();

        try
        {
            _workTypeStorage.UpdateElement(model);
        }
        catch (Exception ex)
        {
            throw new StorageException("Error updating work type", ex);
        }
    }

    public void DeleteWorkType(string id)
    {
        _logger.LogInformation("DeleteWorkType called with id: {Id}", id);

        if (id.IsEmpty())
            throw new ArgumentNullException(nameof(id));

        if (!id.IsGuid())
            throw new ValidationException("Id is not a unique identifier");

        try
        {
            _workTypeStorage.DeleteElement(id);
        }
        catch (Exception ex)
        {
            throw new StorageException("Error deleting work type", ex);
        }
    }
}