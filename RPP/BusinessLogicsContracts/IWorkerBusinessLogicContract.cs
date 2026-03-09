using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RPP.DataModels;
using RPP.Enums;

namespace RPP.BusinessLogicsContracts;

public interface IWorkerBusinessLogicContract
{
    List<WorkerDataModel> GetAllWorkers(bool onlyActive = true);
    List<WorkerDataModel> GetWorkersByPost(WorkerPost post, bool onlyActive = true);
    List<WorkerDataModel> GetWorkersByBirthDate(DateTime fromDate, DateTime toDate, bool onlyActive = true);
    List<WorkerDataModel> GetWorkersByHireDate(DateTime fromDate, DateTime toDate, bool onlyActive = true);
    WorkerDataModel GetWorkerByData(string data);
    void InsertWorker(WorkerDataModel model);
    void UpdateWorker(WorkerDataModel model);
    void DeleteWorker(string id);
}