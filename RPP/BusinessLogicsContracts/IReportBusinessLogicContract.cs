using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RPP.DataModels;

namespace RPP.BusinessLogicsContracts;

public interface IReportBusinessLogicContract
{
    List<ReportDataModel> GetAllReports(DateTime? fromDate = null, DateTime? toDate = null);
    List<ReportDataModel> GetReportsByHome(string homeId);
    List<ReportDataModel> GetReportsByWorker(string workerId);
    List<ReportDataModel> GetReportsByWorkType(string workTypeId);
    List<ReportDataModel> GetReportsByTool(string toolId);
    ReportDataModel GetReportById(string id);
    void InsertReport(ReportDataModel model);
    void UpdateReport(ReportDataModel model);
    void CancelReport(string id);
    double CalculateWorkerSalary(string workerId, DateTime fromDate, DateTime toDate);
}