using Microsoft.Extensions.Logging;
using RPP.BusinessLogicsContracts;
using RPP.DataModels;
using RPP.Enums;
using RPP.Exceptions;
using RPP.Extensions;
using RPP.StoragesContracts;

namespace RPP.Implementations;

public class HomeBusinessLogicContract : IHomeBusinessLogicContract
{
    private readonly ILogger<HomeBusinessLogicContract> _logger;
    private readonly IHomeStorageContract _homeStorage;

    public HomeBusinessLogicContract(
        IHomeStorageContract homeStorage,
        ILogger<HomeBusinessLogicContract> logger)
    {
        _homeStorage = homeStorage;
        _logger = logger;
    }

    public List<HomeDataModel> GetAllHomes()
    {
        _logger.LogInformation("GetAllHomes called");

        var homes = _homeStorage.GetList();
        if (homes == null)
            throw new NullListException();

        return homes;
    }

    public List<HomeDataModel> GetHomesByClient(string clientId)
    {
        _logger.LogInformation("GetHomesByClient called with clientId: {ClientId}", clientId);

        if (clientId.IsEmpty())
            throw new ArgumentNullException(nameof(clientId));

        if (!clientId.IsGuid())
            throw new ValidationException("ClientId is not a unique identifier");

        var homes = _homeStorage.GetListByClient(clientId);
        if (homes == null)
            throw new NullListException();

        return homes;
    }

    public List<HomeDataModel> GetHomesByStatus(HomeStatus status)
    {
        _logger.LogInformation("GetHomesByStatus called with status: {Status}", status);

        if (status == HomeStatus.None)
            throw new ValidationException("Status cannot be None");

        var homes = _homeStorage.GetListByStatus(status);
        if (homes == null)
            throw new NullListException();

        return homes;
    }

    public List<HomeDataModel> GetHomesByType(HomeType type)
    {
        _logger.LogInformation("GetHomesByType called with type: {Type}", type);

        if (type == HomeType.None)
            throw new ValidationException("Type cannot be None");

        var homes = _homeStorage.GetListByType(type);
        if (homes == null)
            throw new NullListException();

        return homes;
    }

    public HomeDataModel GetHomeByData(string data)
    {
        _logger.LogInformation("GetHomeByData called with data: {Data}", data);

        if (data.IsEmpty())
            throw new ArgumentNullException(nameof(data));

        HomeDataModel? home = null;

        if (data.IsGuid())
        {
            home = _homeStorage.GetElementById(data);
        }
        else
        {
            home = _homeStorage.GetElementByAddress(data);
        }

        return home ?? throw new ElementNotFoundException(data);
    }

    public void InsertHome(HomeDataModel model)
    {
        _logger.LogInformation("InsertHome called");

        ArgumentNullException.ThrowIfNull(model);

        model.Validate();

        try
        {
            _homeStorage.AddElement(model);
        }
        catch (Exception ex)
        {
            throw new StorageException("Error inserting home", ex);
        }
    }

    public void UpdateHome(HomeDataModel model)
    {
        _logger.LogInformation("UpdateHome called");

        ArgumentNullException.ThrowIfNull(model);

        model.Validate();

        try
        {
            _homeStorage.UpdateElement(model);
        }
        catch (Exception ex)
        {
            throw new StorageException("Error updating home", ex);
        }
    }

    public void DeleteHome(string id)
    {
        _logger.LogInformation("DeleteHome called with id: {Id}", id);

        if (id.IsEmpty())
            throw new ArgumentNullException(nameof(id));

        if (!id.IsGuid())
            throw new ValidationException("Id is not a unique identifier");

        try
        {
            _homeStorage.DeleteElement(id);
        }
        catch (Exception ex)
        {
            throw new StorageException("Error deleting home", ex);
        }
    }
}