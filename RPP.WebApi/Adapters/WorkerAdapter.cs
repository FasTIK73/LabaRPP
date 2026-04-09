using AutoMapper;
using RPP.BusinessLogicsContracts;
using RPP.Common.Exceptions;
using RPP.DataModels;
using RPP.WebApi.Infrastructure;
using RPP.WebApi.Models.BindingModels;
using RPP.WebApi.Models.ViewModels;

namespace RPP.WebApi.Adapters;

/// <summary>
/// Адаптер для работы с работниками
/// </summary>
public class WorkerAdapter
{
    private readonly IWorkerBusinessLogicContract _businessLogic;
    private readonly IMapper _mapper;
    private readonly ILogger<WorkerAdapter> _logger;

    public WorkerAdapter(
        IWorkerBusinessLogicContract businessLogic,
        IMapper mapper,
        ILogger<WorkerAdapter> logger)
    {
        _businessLogic = businessLogic;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Получить всех работников
    /// </summary>
    public WorkerOperationResponse GetAll(bool onlyActive = true)
    {
        try
        {
            var workers = _businessLogic.GetAllWorkers(onlyActive);
            var viewModels = _mapper.Map<List<WorkerViewModel>>(workers);
            return WorkerOperationResponse.OK(viewModels);
        }
        catch (NullListException ex)
        {
            _logger.LogError(ex, "NullListException");
            return WorkerOperationResponse.NotFound("Список работников не инициализирован");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return WorkerOperationResponse.InternalServerError($"Ошибка хранилища: {ex.InnerException?.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return WorkerOperationResponse.InternalServerError(ex.Message);
        }
    }

    /// <summary>
    /// Получить работника по данным (ID, телефон или email)
    /// </summary>
    public WorkerOperationResponse GetByData(string data)
    {
        try
        {
            var worker = _businessLogic.GetWorkerByData(data);
            var viewModel = _mapper.Map<WorkerViewModel>(worker);
            return WorkerOperationResponse.OK(viewModel);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return WorkerOperationResponse.BadRequest("Данные для поиска пусты");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return WorkerOperationResponse.NotFound($"Работник с данными '{data}' не найден");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return WorkerOperationResponse.InternalServerError($"Ошибка хранилища: {ex.InnerException?.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return WorkerOperationResponse.InternalServerError(ex.Message);
        }
    }

    /// <summary>
    /// Создать нового работника
    /// </summary>
    public WorkerOperationResponse Create(WorkerBindingModel model)
    {
        try
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var dataModel = _mapper.Map<WorkerDataModel>(model);
            _businessLogic.InsertWorker(dataModel);
            return WorkerOperationResponse.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return WorkerOperationResponse.BadRequest("Данные работника пусты");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return WorkerOperationResponse.BadRequest($"Некорректные данные: {ex.Message}");
        }
        catch (ElementExistsException ex)
        {
            _logger.LogError(ex, "ElementExistsException");
            return WorkerOperationResponse.BadRequest(ex.Message);
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return WorkerOperationResponse.BadRequest($"Ошибка хранилища: {ex.InnerException?.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return WorkerOperationResponse.InternalServerError(ex.Message);
        }
    }

    /// <summary>
    /// Обновить данные работника
    /// </summary>
    public WorkerOperationResponse Update(WorkerBindingModel model)
    {
        try
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (string.IsNullOrEmpty(model.Id))
                throw new ValidationException("ID работника обязателен для обновления");

            var dataModel = _mapper.Map<WorkerDataModel>(model);
            _businessLogic.UpdateWorker(dataModel);
            return WorkerOperationResponse.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return WorkerOperationResponse.BadRequest("Данные работника пусты");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return WorkerOperationResponse.BadRequest($"Некорректные данные: {ex.Message}");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return WorkerOperationResponse.BadRequest($"Работник с ID {model.Id} не найден");
        }
        catch (ElementExistsException ex)
        {
            _logger.LogError(ex, "ElementExistsException");
            return WorkerOperationResponse.BadRequest(ex.Message);
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return WorkerOperationResponse.BadRequest($"Ошибка хранилища: {ex.InnerException?.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return WorkerOperationResponse.InternalServerError(ex.Message);
        }
    }

    /// <summary>
    /// Удалить работника
    /// </summary>
    public WorkerOperationResponse Delete(string id)
    {
        try
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));

            _businessLogic.DeleteWorker(id);
            return WorkerOperationResponse.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return WorkerOperationResponse.BadRequest("ID работника пуст");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return WorkerOperationResponse.BadRequest($"Некорректный ID: {ex.Message}");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return WorkerOperationResponse.BadRequest($"Работник с ID {id} не найден");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return WorkerOperationResponse.BadRequest($"Ошибка хранилища: {ex.InnerException?.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return WorkerOperationResponse.InternalServerError(ex.Message);
        }
    }

    /// <summary>
    /// Получить работников по должности
    /// </summary>
    public WorkerOperationResponse GetByPost(RPP.Common.Enums.WorkerPost post, bool onlyActive = true)
    {
        try
        {
            var workers = _businessLogic.GetWorkersByPost(post, onlyActive);
            var viewModels = _mapper.Map<List<WorkerViewModel>>(workers);
            return WorkerOperationResponse.OK(viewModels);
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return WorkerOperationResponse.BadRequest($"Некорректные данные: {ex.Message}");
        }
        catch (NullListException ex)
        {
            _logger.LogError(ex, "NullListException");
            return WorkerOperationResponse.NotFound("Список работников не инициализирован");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return WorkerOperationResponse.InternalServerError($"Ошибка хранилища: {ex.InnerException?.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return WorkerOperationResponse.InternalServerError(ex.Message);
        }
    }
}