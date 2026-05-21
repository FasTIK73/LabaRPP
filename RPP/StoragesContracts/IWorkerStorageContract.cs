using RPP.Common.Enums;
using RPP.DataModels;

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

    // НОВЫЙ МЕТОД ДЛЯ 5 ЛАБЫ
    int GetWorkerTrend(DateTime fromPeriod, DateTime toPeriod);
}