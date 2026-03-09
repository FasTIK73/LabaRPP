using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RPP.DataModels;

namespace RPP.BusinessLogicsContracts;

public interface IWorkTypeBusinessLogicContract
{
    List<WorkTypeDataModel> GetAllWorkTypes();
    WorkTypeDataModel GetWorkTypeByData(string data);
    List<WorkTypeDataModel> GetPriceHistory(string id);
    void InsertWorkType(WorkTypeDataModel model);
    void UpdateWorkType(WorkTypeDataModel model);
    void DeleteWorkType(string id);
}