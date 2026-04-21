using AutoMapper;
using RPP.BusinessLogicsContracts;
using RPP.Common.Exceptions;
using RPP.DataModels;
using RPP.WebApi.Infrastructure;
using RPP.WebApi.Models.BindingModels;
using RPP.WebApi.Models.ViewModels;

namespace RPP.WebApi.Adapters;

/// <summary>
/// Адаптер для работы с отчетами
/// </summary>
public class ReportAdapter
{
    private readonly IReportBusinessLogicContract _businessLogic;
    private readonly IWorkerBusinessLogicContract _workerBusinessLogic;
    private readonly IMapper _mapper;
    private readonly ILogger<ReportAdapter> _logger;

    public ReportAdapter(
        IReportBusinessLogicContract businessLogic,
        IWorkerBusinessLogicContract workerBusinessLogic,
        IMapper mapper,
        ILogger<ReportAdapter> logger)
    {
        _businessLogic = businessLogic;
        _workerBusinessLogic = workerBusinessLogic;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Получить все отчеты за период
    /// </summary>
    public ReportOperationResponse GetAll(DateTime? fromDate = null, DateTime? toDate = null)
    {
        try
        {
            var reports = _businessLogic.GetAllReports(fromDate, toDate);
            var viewModels = _mapper.Map<List<ReportViewModel>>(reports);
            return ReportOperationResponse.OK(viewModels);
        }
        catch (IncorrectDatesException ex)
        {
            _logger.LogError(ex, "IncorrectDatesException");
            return ReportOperationResponse.BadRequest($"Некорректный период дат: {ex.Message}");
        }
        catch (NullListException ex)
        {
            _logger.LogError(ex, "NullListException");
            return ReportOperationResponse.NotFound("Список отчетов не инициализирован");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return ReportOperationResponse.InternalServerError($"Ошибка хранилища: {ex.InnerException?.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return ReportOperationResponse.InternalServerError(ex.Message);
        }
    }

    /// <summary>
    /// Получить отчет по ID
    /// </summary>
    public ReportOperationResponse GetById(string id)
    {
        try
        {
            var report = _businessLogic.GetReportById(id);
            var viewModel = _mapper.Map<ReportViewModel>(report);
            return ReportOperationResponse.OK(viewModel);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return ReportOperationResponse.BadRequest("ID отчета пуст");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return ReportOperationResponse.BadRequest($"Некорректный ID: {ex.Message}");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return ReportOperationResponse.NotFound($"Отчет с ID {id} не найден");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return ReportOperationResponse.InternalServerError($"Ошибка хранилища: {ex.InnerException?.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return ReportOperationResponse.InternalServerError(ex.Message);
        }
    }

    /// <summary>
    /// Получить отчеты по помещению
    /// </summary>
    public ReportOperationResponse GetByHome(string homeId)
    {
        try
        {
            var reports = _businessLogic.GetReportsByHome(homeId);
            var viewModels = _mapper.Map<List<ReportViewModel>>(reports);
            return ReportOperationResponse.OK(viewModels);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return ReportOperationResponse.BadRequest("ID помещения пуст");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return ReportOperationResponse.BadRequest($"Некорректный ID: {ex.Message}");
        }
        catch (NullListException ex)
        {
            _logger.LogError(ex, "NullListException");
            return ReportOperationResponse.NotFound("Список отчетов не инициализирован");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return ReportOperationResponse.InternalServerError($"Ошибка хранилища: {ex.InnerException?.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return ReportOperationResponse.InternalServerError(ex.Message);
        }
    }

    /// <summary>
    /// Получить отчеты по работнику
    /// </summary>
    public ReportOperationResponse GetByWorker(string workerId)
    {
        try
        {
            var reports = _businessLogic.GetReportsByWorker(workerId);
            var viewModels = _mapper.Map<List<ReportViewModel>>(reports);
            return ReportOperationResponse.OK(viewModels);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return ReportOperationResponse.BadRequest("ID работника пуст");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return ReportOperationResponse.BadRequest($"Некорректный ID: {ex.Message}");
        }
        catch (NullListException ex)
        {
            _logger.LogError(ex, "NullListException");
            return ReportOperationResponse.NotFound("Список отчетов не инициализирован");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return ReportOperationResponse.InternalServerError($"Ошибка хранилища: {ex.InnerException?.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return ReportOperationResponse.InternalServerError(ex.Message);
        }
    }

    /// <summary>
    /// Получить отчеты по типу работы
    /// </summary>
    public ReportOperationResponse GetByWorkType(string workTypeId)
    {
        try
        {
            var reports = _businessLogic.GetReportsByWorkType(workTypeId);
            var viewModels = _mapper.Map<List<ReportViewModel>>(reports);
            return ReportOperationResponse.OK(viewModels);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return ReportOperationResponse.BadRequest("ID типа работы пуст");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return ReportOperationResponse.BadRequest($"Некорректный ID: {ex.Message}");
        }
        catch (NullListException ex)
        {
            _logger.LogError(ex, "NullListException");
            return ReportOperationResponse.NotFound("Список отчетов не инициализирован");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return ReportOperationResponse.InternalServerError($"Ошибка хранилища: {ex.InnerException?.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return ReportOperationResponse.InternalServerError(ex.Message);
        }
    }

    /// <summary>
    /// Получить отчеты по инструменту
    /// </summary>
    public ReportOperationResponse GetByTool(string toolId)
    {
        try
        {
            var reports = _businessLogic.GetReportsByTool(toolId);
            var viewModels = _mapper.Map<List<ReportViewModel>>(reports);
            return ReportOperationResponse.OK(viewModels);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return ReportOperationResponse.BadRequest("ID инструмента пуст");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return ReportOperationResponse.BadRequest($"Некорректный ID: {ex.Message}");
        }
        catch (NullListException ex)
        {
            _logger.LogError(ex, "NullListException");
            return ReportOperationResponse.NotFound("Список отчетов не инициализирован");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return ReportOperationResponse.InternalServerError($"Ошибка хранилища: {ex.InnerException?.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return ReportOperationResponse.InternalServerError(ex.Message);
        }
    }

    /// <summary>
    /// Создать новый отчет
    /// </summary>
    public ReportOperationResponse Create(ReportBindingModel model)
    {
        try
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var dataModel = _mapper.Map<ReportDataModel>(model);
            _businessLogic.InsertReport(dataModel);
            return ReportOperationResponse.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return ReportOperationResponse.BadRequest("Данные отчета пусты");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return ReportOperationResponse.BadRequest($"Некорректные данные: {ex.Message}");
        }
        catch (ElementExistsException ex)
        {
            _logger.LogError(ex, "ElementExistsException");
            return ReportOperationResponse.BadRequest(ex.Message);
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return ReportOperationResponse.BadRequest($"Ошибка хранилища: {ex.InnerException?.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return ReportOperationResponse.InternalServerError(ex.Message);
        }
    }

    /// <summary>
    /// Обновить отчет
    /// </summary>
    public ReportOperationResponse Update(ReportBindingModel model)
    {
        try
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (string.IsNullOrEmpty(model.Id))
                throw new ValidationException("ID отчета обязателен для обновления");

            var dataModel = _mapper.Map<ReportDataModel>(model);
            _businessLogic.UpdateReport(dataModel);
            return ReportOperationResponse.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return ReportOperationResponse.BadRequest("Данные отчета пусты");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return ReportOperationResponse.BadRequest($"Некорректные данные: {ex.Message}");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return ReportOperationResponse.BadRequest($"Отчет с ID {model.Id} не найден");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return ReportOperationResponse.BadRequest($"Ошибка хранилища: {ex.InnerException?.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return ReportOperationResponse.InternalServerError(ex.Message);
        }
    }

    /// <summary>
    /// Отменить отчет (удалить)
    /// </summary>
    public ReportOperationResponse Cancel(string id)
    {
        try
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));

            _businessLogic.CancelReport(id);
            return ReportOperationResponse.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return ReportOperationResponse.BadRequest("ID отчета пуст");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return ReportOperationResponse.BadRequest($"Некорректный ID: {ex.Message}");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return ReportOperationResponse.BadRequest($"Отчет с ID {id} не найден");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return ReportOperationResponse.BadRequest($"Ошибка хранилища: {ex.InnerException?.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return ReportOperationResponse.InternalServerError(ex.Message);
        }
    }

    /// <summary>
    /// Рассчитать зарплату работника
    /// </summary>
    public SalaryOperationResponse CalculateSalary(string workerId, DateTime fromDate, DateTime toDate)
    {
        try
        {
            var salary = _businessLogic.CalculateWorkerSalary(workerId, fromDate, toDate);

            // Получаем информацию о работнике через IWorkerBusinessLogicContract
            WorkerDataModel worker = null;
            try
            {
                worker = _workerBusinessLogic.GetWorkerByData(workerId);
            }
            catch (ElementNotFoundException)
            {
                // Работник не найден, но продолжим с пустым именем
            }

            var result = new SalaryViewModel
            {
                WorkerId = workerId,
                WorkerName = worker?.FullName ?? string.Empty,
                Salary = salary,
                FromDate = fromDate,
                ToDate = toDate
            };

            return SalaryOperationResponse.OK(result);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return SalaryOperationResponse.BadRequest("ID работника пуст");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return SalaryOperationResponse.BadRequest($"Некорректные данные: {ex.Message}");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return SalaryOperationResponse.BadRequest($"Работник с ID {workerId} не найден");
        }
        catch (IncorrectDatesException ex)
        {
            _logger.LogError(ex, "IncorrectDatesException");
            return SalaryOperationResponse.BadRequest($"Некорректный период дат: {ex.Message}");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return SalaryOperationResponse.InternalServerError($"Ошибка хранилища: {ex.InnerException?.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return SalaryOperationResponse.InternalServerError(ex.Message);
        }
    }
}