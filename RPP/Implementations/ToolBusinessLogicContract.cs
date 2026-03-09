using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;
using RPP.BusinessLogicsContracts;
using RPP.DataModels;
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
        throw new NotImplementedException();
    }

    public ToolDataModel GetToolByData(string data)
    {
        throw new NotImplementedException();
    }

    public void InsertTool(ToolDataModel model)
    {
        throw new NotImplementedException();
    }

    public void UpdateTool(ToolDataModel model)
    {
        throw new NotImplementedException();
    }

    public void DeleteTool(string id)
    {
        throw new NotImplementedException();
    }
}