using Microsoft.Extensions.Logging;
using RPP.BusinessLogicsContracts;
using RPP.DataModels;
using RPP.StoragesContracts;

using Microsoft.Extensions.Logging;
using RPP.BusinessLogicsContracts;
using RPP.DataModels;
using RPP.Exceptions;
using RPP.Extensions;
using RPP.StoragesContracts;
using System.Text.RegularExpressions;

namespace RPP.Implementations;

public class ClientBusinessLogicContract : IClientBusinessLogicContract
{
    private readonly ILogger<ClientBusinessLogicContract> _logger;
    private readonly IClientStorageContract _clientStorage;

    public ClientBusinessLogicContract(
        IClientStorageContract clientStorage,
        ILogger<ClientBusinessLogicContract> logger)
    {
        _clientStorage = clientStorage;
        _logger = logger;
    }

    public List<ClientDataModel> GetAllClients()
    {
        _logger.LogInformation("GetAllClients called");

        var clients = _clientStorage.GetList();
        if (clients == null)
            throw new NullListException();

        return clients;
    }

    public ClientDataModel GetClientByData(string data)
    {
        _logger.LogInformation("GetClientByData called with data: {Data}", data);

        if (data.IsEmpty())
            throw new ArgumentNullException(nameof(data));

        ClientDataModel? client = null;

        if (data.IsGuid())
        {
            client = _clientStorage.GetElementById(data);
        }
        else if (Regex.IsMatch(data, @"^((8|\+7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?[\d\- ]{7,10}$"))
        {
            client = _clientStorage.GetElementByPhone(data);
        }
        else
        {
            client = _clientStorage.GetElementByName(data);
        }

        return client ?? throw new ElementNotFoundException(data);
    }

    public void InsertClient(ClientDataModel model)
    {
        _logger.LogInformation("InsertClient called");

        ArgumentNullException.ThrowIfNull(model);

        model.Validate();

        try
        {
            _clientStorage.AddElement(model);
        }
        catch (Exception ex)
        {
            throw new StorageException("Error inserting client", ex);
        }
    }

    public void UpdateClient(ClientDataModel model)
    {
        _logger.LogInformation("UpdateClient called");

        ArgumentNullException.ThrowIfNull(model);

        model.Validate();

        try
        {
            _clientStorage.UpdateElement(model);
        }
        catch (Exception ex)
        {
            throw new StorageException("Error updating client", ex);
        }
    }

    public void DeleteClient(string id)
    {
        _logger.LogInformation("DeleteClient called with id: {Id}", id);

        if (id.IsEmpty())
            throw new ArgumentNullException(nameof(id));

        if (!id.IsGuid())
            throw new ValidationException("Id is not a unique identifier");

        try
        {
            _clientStorage.DeleteElement(id);
        }
        catch (Exception ex)
        {
            throw new StorageException("Error deleting client", ex);
        }
    }
}