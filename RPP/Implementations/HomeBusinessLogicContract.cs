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
        throw new NotImplementedException();
    }

    public List<HomeDataModel> GetHomesByClient(string clientId)
    {
        throw new NotImplementedException();
    }

    public List<HomeDataModel> GetHomesByStatus(HomeStatus status)
    {
        throw new NotImplementedException();
    }

    public List<HomeDataModel> GetHomesByType(HomeType type)
    {
        throw new NotImplementedException();
    }

    public HomeDataModel GetHomeByData(string data)
    {
        throw new NotImplementedException();
    }

    public void InsertHome(HomeDataModel model)
    {
        throw new NotImplementedException();
    }

    public void UpdateHome(HomeDataModel model)
    {
        throw new NotImplementedException();
    }

    public void DeleteHome(string id)
    {
        throw new NotImplementedException();
    }
}