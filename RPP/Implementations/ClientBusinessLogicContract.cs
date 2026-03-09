using Microsoft.Extensions.Logging;
using RPP.BusinessLogicsContracts;
using RPP.DataModels;
using RPP.StoragesContracts;

using Microsoft.Extensions.Logging;
using RPP.BusinessLogicsContracts;
using RPP.DataModels;
using RPP.StoragesContracts;

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
        throw new NotImplementedException();
    }

    public ClientDataModel GetClientByData(string data)
    {
        throw new NotImplementedException();
    }

    public void InsertClient(ClientDataModel model)
    {
        throw new NotImplementedException();
    }

    public void UpdateClient(ClientDataModel model)
    {
        throw new NotImplementedException();
    }

    public void DeleteClient(string id)
    {
        throw new NotImplementedException();
    }
}