using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RPP.DataModels;

namespace RPP.StoragesContracts;

public interface IToolStorageContract
{
    List<ToolDataModel> GetList(bool onlyAvailable = true);
    ToolDataModel? GetElementById(string id);
    ToolDataModel? GetElementByName(string name);
    ToolDataModel? GetElementByPreviousName(string previousName);
    void AddElement(ToolDataModel element);
    void UpdateElement(ToolDataModel element);
    void DeleteElement(string id);
}