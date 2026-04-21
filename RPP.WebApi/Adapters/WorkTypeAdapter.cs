using AutoMapper;
using RPP.BusinessLogicsContracts;
using RPP.Common.Exceptions;
using RPP.DataModels;
using RPP.WebApi.Infrastructure;
using RPP.WebApi.Models.BindingModels;
using RPP.WebApi.Models.ViewModels;

namespace RPP.WebApi.Adapters;

/// <summary>
/// Адаптер для работы с типами работ
/// </summary>
public class WorkTypeAdapter
{
    private readonly IWorkTypeBusinessLogicContract _businessLogic;
    private readonly IMapper _mapper;
    private readonly ILogger<WorkTypeAdapter> _logger;

    public WorkTypeAdapter(
        IWorkTypeBusinessLogicContract businessLogic,
        IMapper mapper,
        ILogger<WorkTypeAdapter> logger)
    {
        _businessLogic = businessLogic;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Получить все типы работ
    /// </summary>
    public WorkTypeOperationResponse GetAll()
    {
        try
        {
            var workTypes = _businessLogic.GetAllWorkTypes();
            var viewModels = _mapper.Map<List<WorkTypeViewModel>>(workTypes);
            return WorkTypeOperationResponse.OK(viewModels);
        }
        catch (NullListException ex)
        {
            _logger.LogError(ex, "NullListException");
            return WorkTypeOperationResponse.NotFound("Список типов работ не инициализирован");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return WorkTypeOperationResponse.InternalServerError($"Ошибка хранилища: {ex.InnerException?.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return WorkTypeOperationResponse.InternalServerError(ex.Message);
        }
    }

    /// <summary>
    /// Получить тип работы по данным (ID или название)
    /// </summary>
    public WorkTypeOperationResponse GetByData(string data)
    {
        try
        {
            var workType = _businessLogic.GetWorkTypeByData(data);
            var viewModel = _mapper.Map<WorkTypeViewModel>(workType);
            return WorkTypeOperationResponse.OK(viewModel);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return WorkTypeOperationResponse.BadRequest("Данные для поиска пусты");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return WorkTypeOperationResponse.NotFound($"Тип работы с данными '{data}' не найден");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return WorkTypeOperationResponse.InternalServerError($"Ошибка хранилища: {ex.InnerException?.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return WorkTypeOperationResponse.InternalServerError(ex.Message);
        }
    }

    /// <summary>
    /// Получить историю изменения цены
    /// </summary>
    public WorkTypeOperationResponse GetPriceHistory(string id)
    {
        try
        {
            var history = _businessLogic.GetPriceHistory(id);
            var viewModels = _mapper.Map<List<WorkTypeViewModel>>(history);
            return WorkTypeOperationResponse.OK(viewModels);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return WorkTypeOperationResponse.BadRequest("ID типа работы пуст");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return WorkTypeOperationResponse.BadRequest($"Некорректный ID: {ex.Message}");
        }
        catch (NullListException ex)
        {
            _logger.LogError(ex, "NullListException");
            return WorkTypeOperationResponse.NotFound("История цен не инициализирована");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return WorkTypeOperationResponse.InternalServerError($"Ошибка хранилища: {ex.InnerException?.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return WorkTypeOperationResponse.InternalServerError(ex.Message);
        }
    }

    /// <summary>
    /// Создать новый тип работы
    /// </summary>
    public WorkTypeOperationResponse Create(WorkTypeBindingModel model)
    {
        try
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var dataModel = _mapper.Map<WorkTypeDataModel>(model);
            _businessLogic.InsertWorkType(dataModel);
            return WorkTypeOperationResponse.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return WorkTypeOperationResponse.BadRequest("Данные типа работы пусты");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return WorkTypeOperationResponse.BadRequest($"Некорректные данные: {ex.Message}");
        }
        catch (ElementExistsException ex)
        {
            _logger.LogError(ex, "ElementExistsException");
            return WorkTypeOperationResponse.BadRequest(ex.Message);
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return WorkTypeOperationResponse.BadRequest($"Ошибка хранилища: {ex.InnerException?.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return WorkTypeOperationResponse.InternalServerError(ex.Message);
        }
    }

    /// <summary>
    /// Обновить данные типа работы
    /// </summary>
    public WorkTypeOperationResponse Update(WorkTypeBindingModel model)
    {
        try
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (string.IsNullOrEmpty(model.Id))
                throw new ValidationException("ID типа работы обязателен для обновления");

            var dataModel = _mapper.Map<WorkTypeDataModel>(model);
            _businessLogic.UpdateWorkType(dataModel);
            return WorkTypeOperationResponse.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return WorkTypeOperationResponse.BadRequest("Данные типа работы пусты");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return WorkTypeOperationResponse.BadRequest($"Некорректные данные: {ex.Message}");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return WorkTypeOperationResponse.BadRequest($"Тип работы с ID {model.Id} не найден");
        }
        catch (ElementExistsException ex)
        {
            _logger.LogError(ex, "ElementExistsException");
            return WorkTypeOperationResponse.BadRequest(ex.Message);
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return WorkTypeOperationResponse.BadRequest($"Ошибка хранилища: {ex.InnerException?.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return WorkTypeOperationResponse.InternalServerError(ex.Message);
        }
    }

    /// <summary>
    /// Удалить тип работы
    /// </summary>
    public WorkTypeOperationResponse Delete(string id)
    {
        try
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));

            _businessLogic.DeleteWorkType(id);
            return WorkTypeOperationResponse.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return WorkTypeOperationResponse.BadRequest("ID типа работы пуст");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return WorkTypeOperationResponse.BadRequest($"Некорректный ID: {ex.Message}");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return WorkTypeOperationResponse.BadRequest($"Тип работы с ID {id} не найден");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return WorkTypeOperationResponse.BadRequest($"Ошибка хранилища: {ex.InnerException?.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return WorkTypeOperationResponse.InternalServerError(ex.Message);
        }
    }
}