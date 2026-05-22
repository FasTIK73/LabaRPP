using AutoMapper;
using RPP.BusinessLogicsContracts;
using RPP.Common.Exceptions;
using RPP.DataModels;
using RPP.WebApi.Infrastructure;
using RPP.WebApi.Models.BindingModels;
using RPP.WebApi.Models.ViewModels;

namespace RPP.WebApi.Adapters;

public class ReportAdapter
{
    private readonly IReportBusinessLogicContract _businessLogic;
    private readonly IMapper _mapper;
    private readonly ILogger<ReportAdapter> _logger;

    public ReportAdapter(IReportBusinessLogicContract businessLogic, IMapper mapper, ILogger<ReportAdapter> logger)
    {
        _businessLogic = businessLogic;
        _mapper = mapper;
        _logger = logger;
    }

    public ReportOperationResponse GetAll(DateTime? fromDate, DateTime? toDate)
    {
        try
        {
            var reports = _businessLogic.GetAllReportsAsync(fromDate, toDate).Result;
            var viewModels = _mapper.Map<List<ReportViewModel>>(reports);
            return ReportOperationResponse.OK(viewModels);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting reports");
            return ReportOperationResponse.InternalServerError(ex.Message);
        }
    }

    public ReportOperationResponse GetById(string id)
    {
        try
        {
            var report = _businessLogic.GetReportByIdAsync(id).Result;
            if (report == null)
                return ReportOperationResponse.NotFound($"Report with id {id} not found");
            var viewModel = _mapper.Map<ReportViewModel>(report);
            return ReportOperationResponse.OK(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting report by id");
            return ReportOperationResponse.InternalServerError(ex.Message);
        }
    }

    public ReportOperationResponse GetByHome(string homeId)
    {
        try
        {
            var reports = _businessLogic.GetReportsByHomeAsync(homeId).Result;
            var viewModels = _mapper.Map<List<ReportViewModel>>(reports);
            return ReportOperationResponse.OK(viewModels);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting reports by home");
            return ReportOperationResponse.InternalServerError(ex.Message);
        }
    }

    public ReportOperationResponse GetByWorker(string workerId)
    {
        try
        {
            var reports = _businessLogic.GetReportsByWorkerAsync(workerId).Result;
            var viewModels = _mapper.Map<List<ReportViewModel>>(reports);
            return ReportOperationResponse.OK(viewModels);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting reports by worker");
            return ReportOperationResponse.InternalServerError(ex.Message);
        }
    }

    public ReportOperationResponse GetByWorkType(string workTypeId)
    {
        try
        {
            var reports = _businessLogic.GetReportsByWorkTypeAsync(workTypeId).Result;
            var viewModels = _mapper.Map<List<ReportViewModel>>(reports);
            return ReportOperationResponse.OK(viewModels);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting reports by work type");
            return ReportOperationResponse.InternalServerError(ex.Message);
        }
    }

    public ReportOperationResponse GetByTool(string toolId)
    {
        try
        {
            var reports = _businessLogic.GetReportsByToolAsync(toolId).Result;
            var viewModels = _mapper.Map<List<ReportViewModel>>(reports);
            return ReportOperationResponse.OK(viewModels);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting reports by tool");
            return ReportOperationResponse.InternalServerError(ex.Message);
        }
    }

    public ReportOperationResponse Create(ReportBindingModel model)
    {
        try
        {
            var dataModel = _mapper.Map<ReportDataModel>(model);
            _businessLogic.InsertReportAsync(dataModel).Wait();
            return ReportOperationResponse.NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating report");
            return ReportOperationResponse.InternalServerError(ex.Message);
        }
    }

    public ReportOperationResponse Update(ReportBindingModel model)
    {
        try
        {
            if (string.IsNullOrEmpty(model.Id))
                return ReportOperationResponse.BadRequest("Id is required");
            var dataModel = _mapper.Map<ReportDataModel>(model);
            _businessLogic.UpdateReportAsync(dataModel).Wait();
            return ReportOperationResponse.NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating report");
            return ReportOperationResponse.InternalServerError(ex.Message);
        }
    }

    public ReportOperationResponse Cancel(string id)
    {
        try
        {
            _businessLogic.CancelReportAsync(id).Wait();
            return ReportOperationResponse.NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error canceling report");
            return ReportOperationResponse.InternalServerError(ex.Message);
        }
    }

    public ReportOperationResponse CalculateSalary(string workerId, DateTime fromDate, DateTime toDate)
    {
        try
        {
            var salary = _businessLogic.CalculateWorkerSalaryAsync(workerId, fromDate, toDate).Result;
            var result = new SalaryViewModel
            {
                WorkerId = workerId,
                WorkerName = "",
                Salary = salary,
                FromDate = fromDate,
                ToDate = toDate
            };
            return ReportOperationResponse.OK(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating salary");
            return ReportOperationResponse.InternalServerError(ex.Message);
        }
    }
}