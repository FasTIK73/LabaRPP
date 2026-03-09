using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RPP.DataModels;

namespace RPP.StoragesContracts;

public interface IWorkTypeStorageContract
{
    List<WorkTypeDataModel> GetList();
    WorkTypeDataModel? GetElementById(string id);
    WorkTypeDataModel? GetElementByName(string name);
    List<WorkTypeDataModel> GetPriceHistory(string id);
    void AddElement(WorkTypeDataModel element);
    void UpdateElement(WorkTypeDataModel element);
    void DeleteElement(string id);
}