using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RPP.DataModels;
using RPP.Enums;

namespace RPP.StoragesContracts;

public interface IWorkerStorageContract
{
    List<WorkerDataModel> GetList(bool onlyActive = true);
    List<WorkerDataModel> GetListByPost(WorkerPost post, bool onlyActive = true);
    List<WorkerDataModel> GetListByBirthDate(DateTime fromDate, DateTime toDate, bool onlyActive = true);
    List<WorkerDataModel> GetListByHireDate(DateTime fromDate, DateTime toDate, bool onlyActive = true);
    WorkerDataModel? GetElementById(string id);
    WorkerDataModel? GetElementByPhone(string phoneNumber);
    WorkerDataModel? GetElementByEmail(string email);
    void AddElement(WorkerDataModel element);
    void UpdateElement(WorkerDataModel element);
    void DeleteElement(string id);
}