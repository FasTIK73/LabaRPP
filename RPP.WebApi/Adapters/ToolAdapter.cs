using AutoMapper;
using RPP.BusinessLogicsContracts;
using RPP.Common.Exceptions;
using RPP.DataModels;
using RPP.WebApi.Infrastructure;
using RPP.WebApi.Models.BindingModels;
using RPP.WebApi.Models.ViewModels;

namespace RPP.WebApi.Adapters;

/// <summary>
/// Адаптер для работы с инструментами
/// </summary>
public class ToolAdapter
{
    private readonly IToolBusinessLogicContract _businessLogic;
    private readonly IMapper _mapper;
    private readonly ILogger<ToolAdapter> _logger;

    public ToolAdapter(
        IToolBusinessLogicContract businessLogic,
        IMapper mapper,
        ILogger<ToolAdapter> logger)
    {
        _businessLogic = businessLogic;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Получить все инструменты
    /// </summary>
    public ToolOperationResponse GetAll(bool onlyAvailable = true)
    {
        try
        {
            var tools = _businessLogic.GetAllTools(onlyAvailable);
            var viewModels = _mapper.Map<List<ToolViewModel>>(tools);
            return ToolOperationResponse.OK(viewModels);
        }
        catch (NullListException ex)
        {
            _logger.LogError(ex, "NullListException");
            return ToolOperationResponse.NotFound("Список инструментов не инициализирован");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return ToolOperationResponse.InternalServerError($"Ошибка хранилища: {ex.InnerException?.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return ToolOperationResponse.InternalServerError(ex.Message);
        }
    }

    /// <summary>
    /// Получить инструмент по данным (ID или название)
    /// </summary>
    public ToolOperationResponse GetByData(string data)
    {
        try
        {
            var tool = _businessLogic.GetToolByData(data);
            var viewModel = _mapper.Map<ToolViewModel>(tool);
            return ToolOperationResponse.OK(viewModel);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return ToolOperationResponse.BadRequest("Данные для поиска пусты");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return ToolOperationResponse.NotFound($"Инструмент с данными '{data}' не найден");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return ToolOperationResponse.InternalServerError($"Ошибка хранилища: {ex.InnerException?.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return ToolOperationResponse.InternalServerError(ex.Message);
        }
    }

    /// <summary>
    /// Создать новый инструмент
    /// </summary>
    public ToolOperationResponse Create(ToolBindingModel model)
    {
        try
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var dataModel = _mapper.Map<ToolDataModel>(model);
            _businessLogic.InsertTool(dataModel);
            return ToolOperationResponse.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return ToolOperationResponse.BadRequest("Данные инструмента пусты");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return ToolOperationResponse.BadRequest($"Некорректные данные: {ex.Message}");
        }
        catch (ElementExistsException ex)
        {
            _logger.LogError(ex, "ElementExistsException");
            return ToolOperationResponse.BadRequest(ex.Message);
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return ToolOperationResponse.BadRequest($"Ошибка хранилища: {ex.InnerException?.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return ToolOperationResponse.InternalServerError(ex.Message);
        }
    }

    /// <summary>
    /// Обновить данные инструмента
    /// </summary>
    public ToolOperationResponse Update(ToolBindingModel model)
    {
        try
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (string.IsNullOrEmpty(model.Id))
                throw new ValidationException("ID инструмента обязателен для обновления");

            var dataModel = _mapper.Map<ToolDataModel>(model);
            _businessLogic.UpdateTool(dataModel);
            return ToolOperationResponse.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return ToolOperationResponse.BadRequest("Данные инструмента пусты");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return ToolOperationResponse.BadRequest($"Некорректные данные: {ex.Message}");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return ToolOperationResponse.BadRequest($"Инструмент с ID {model.Id} не найден");
        }
        catch (ElementExistsException ex)
        {
            _logger.LogError(ex, "ElementExistsException");
            return ToolOperationResponse.BadRequest(ex.Message);
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return ToolOperationResponse.BadRequest($"Ошибка хранилища: {ex.InnerException?.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return ToolOperationResponse.InternalServerError(ex.Message);
        }
    }

    /// <summary>
    /// Удалить инструмент
    /// </summary>
    public ToolOperationResponse Delete(string id)
    {
        try
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));

            _businessLogic.DeleteTool(id);
            return ToolOperationResponse.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return ToolOperationResponse.BadRequest("ID инструмента пуст");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return ToolOperationResponse.BadRequest($"Некорректный ID: {ex.Message}");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return ToolOperationResponse.BadRequest($"Инструмент с ID {id} не найден");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return ToolOperationResponse.BadRequest($"Ошибка хранилища: {ex.InnerException?.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return ToolOperationResponse.InternalServerError(ex.Message);
        }
    }
}