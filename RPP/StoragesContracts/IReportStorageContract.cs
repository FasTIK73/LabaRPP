using RPP.DataModels;

namespace RPP.StoragesContracts;

public interface IReportStorageContract
{
    List<ReportDataModel> GetList(DateTime? fromDate = null, DateTime? toDate = null);
    List<ReportDataModel> GetListByHome(string homeId);
    List<ReportDataModel> GetListByWorker(string workerId);
    List<ReportDataModel> GetListByWorkType(string workTypeId);
    List<ReportDataModel> GetListByTool(string toolId);
    ReportDataModel? GetElementById(string id);
    void AddElement(ReportDataModel element);
    void UpdateElement(ReportDataModel element);
    void DeleteElement(string id);

    // НОВЫЙ МЕТОД ДЛЯ 6 ЛАБЫ
    List<ReportDataModel> GetList(DateTime? fromDate = null, DateTime? toDate = null, string? workerId = null);
}