using Microsoft.Extensions.Logging;
using RPP.BusinessLogicsContracts;
using RPP.DataModels;
using RPP.Exceptions;
using RPP.Extensions;
using RPP.StoragesContracts;

namespace RPP.Implementations;

public class ToolBusinessLogicContract : IToolBusinessLogicContract
{
    private readonly ILogger<ToolBusinessLogicContract> _logger;
    private readonly IToolStorageContract _toolStorage;

    public ToolBusinessLogicContract(
        IToolStorageContract toolStorage,
        ILogger<ToolBusinessLogicContract> logger)
    {
        _toolStorage = toolStorage;
        _logger = logger;
    }

    public List<ToolDataModel> GetAllTools(bool onlyAvailable = true)
    {
        _logger.LogInformation("GetAllTools called with onlyAvailable: {OnlyAvailable}", onlyAvailable);

        var tools = _toolStorage.GetList(onlyAvailable);
        if (tools == null)
            throw new NullListException();

        return tools;
    }

    public ToolDataModel GetToolByData(string data)
    {
        _logger.LogInformation("GetToolByData called with data: {Data}", data);

        if (data.IsEmpty())
            throw new ArgumentNullException(nameof(data));

        ToolDataModel? tool = null;

        if (data.IsGuid())
        {
            tool = _toolStorage.GetElementById(data);
        }
        else
        {
            tool = _toolStorage.GetElementByName(data);
            tool ??= _toolStorage.GetElementByPreviousName(data);
        }

        return tool ?? throw new ElementNotFoundException(data);
    }

    public void InsertTool(ToolDataModel model)
    {
        _logger.LogInformation("InsertTool called");

        ArgumentNullException.ThrowIfNull(model);

        model.Validate();

        try
        {
            _toolStorage.AddElement(model);
        }
        catch (Exception ex)
        {
            throw new StorageException("Error inserting tool", ex);
        }
    }

    public void UpdateTool(ToolDataModel model)
    {
        _logger.LogInformation("UpdateTool called");

        ArgumentNullException.ThrowIfNull(model);

        model.Validate();

        try
        {
            _toolStorage.UpdateElement(model);
        }
        catch (Exception ex)
        {
            throw new StorageException("Error updating tool", ex);
        }
    }

    public void DeleteTool(string id)
    {
        _logger.LogInformation("DeleteTool called with id: {Id}", id);

        if (id.IsEmpty())
            throw new ArgumentNullException(nameof(id));

        if (!id.IsGuid())
            throw new ValidationException("Id is not a unique identifier");

        try
        {
            _toolStorage.DeleteElement(id);
        }
        catch (Exception ex)
        {
            throw new StorageException("Error deleting tool", ex);
        }
    }
}   