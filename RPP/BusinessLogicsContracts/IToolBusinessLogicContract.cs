using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RPP.DataModels;

namespace RPP.BusinessLogicsContracts;

public interface IToolBusinessLogicContract
{
    List<ToolDataModel> GetAllTools(bool onlyAvailable = true);
    ToolDataModel GetToolByData(string data);
    void InsertTool(ToolDataModel model);
    void UpdateTool(ToolDataModel model);
    void DeleteTool(string id);
}