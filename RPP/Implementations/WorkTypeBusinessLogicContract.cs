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
        throw new NotImplementedException();
    }

    public WorkTypeDataModel GetWorkTypeByData(string data)
    {
        throw new NotImplementedException();
    }

    public List<WorkTypeDataModel> GetPriceHistory(string id)
    {
        throw new NotImplementedException();
    }

    public void InsertWorkType(WorkTypeDataModel model)
    {
        throw new NotImplementedException();
    }

    public void UpdateWorkType(WorkTypeDataModel model)
    {
        throw new NotImplementedException();
    }

    public void DeleteWorkType(string id)
    {
        throw new NotImplementedException();
    }
}