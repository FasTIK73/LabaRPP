using AutoMapper;
using RPP.BusinessLogicsContracts;
using RPP.Common.Exceptions;
using RPP.DataModels;
using RPP.WebApi.Infrastructure;
using RPP.WebApi.Models.BindingModels;
using RPP.WebApi.Models.ViewModels;

namespace RPP.WebApi.Adapters;

/// <summary>
/// Адаптер для работы с клиентами
/// </summary>
public class ClientAdapter
{
    private readonly IClientBusinessLogicContract _businessLogic;
    private readonly IMapper _mapper;
    private readonly ILogger<ClientAdapter> _logger;

    public ClientAdapter(
        IClientBusinessLogicContract businessLogic,
        IMapper mapper,
        ILogger<ClientAdapter> logger)
    {
        _businessLogic = businessLogic;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Получить всех клиентов
    /// </summary>
    public ClientOperationResponse GetAll()
    {
        try
        {
            var clients = _businessLogic.GetAllClients();
            var viewModels = _mapper.Map<List<ClientViewModel>>(clients);
            return ClientOperationResponse.OK(viewModels);
        }
        catch (NullListException ex)
        {
            _logger.LogError(ex, "NullListException");
            return ClientOperationResponse.NotFound("Список клиентов не инициализирован");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return ClientOperationResponse.InternalServerError($"Ошибка хранилища: {ex.InnerException?.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return ClientOperationResponse.InternalServerError(ex.Message);
        }
    }

    /// <summary>
    /// Получить клиента по данным (ID, телефон или ФИО)
    /// </summary>
    public ClientOperationResponse GetByData(string data)
    {
        try
        {
            var client = _businessLogic.GetClientByData(data);
            var viewModel = _mapper.Map<ClientViewModel>(client);
            return ClientOperationResponse.OK(viewModel);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return ClientOperationResponse.BadRequest("Данные для поиска пусты");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return ClientOperationResponse.NotFound($"Клиент с данными '{data}' не найден");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return ClientOperationResponse.InternalServerError($"Ошибка хранилища: {ex.InnerException?.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return ClientOperationResponse.InternalServerError(ex.Message);
        }
    }

    /// <summary>
    /// Создать нового клиента
    /// </summary>
    public ClientOperationResponse Create(ClientBindingModel model)
    {
        try
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var dataModel = _mapper.Map<ClientDataModel>(model);
            _businessLogic.InsertClient(dataModel);
            return ClientOperationResponse.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return ClientOperationResponse.BadRequest("Данные клиента пусты");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return ClientOperationResponse.BadRequest($"Некорректные данные: {ex.Message}");
        }
        catch (ElementExistsException ex)
        {
            _logger.LogError(ex, "ElementExistsException");
            return ClientOperationResponse.BadRequest(ex.Message);
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return ClientOperationResponse.BadRequest($"Ошибка хранилища: {ex.InnerException?.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return ClientOperationResponse.InternalServerError(ex.Message);
        }
    }

    /// <summary>
    /// Обновить данные клиента
    /// </summary>
    public ClientOperationResponse Update(ClientBindingModel model)
    {
        try
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (string.IsNullOrEmpty(model.Id))
                throw new ValidationException("ID клиента обязателен для обновления");

            var dataModel = _mapper.Map<ClientDataModel>(model);
            _businessLogic.UpdateClient(dataModel);
            return ClientOperationResponse.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return ClientOperationResponse.BadRequest("Данные клиента пусты");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return ClientOperationResponse.BadRequest($"Некорректные данные: {ex.Message}");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return ClientOperationResponse.BadRequest($"Клиент с ID {model.Id} не найден");
        }
        catch (ElementExistsException ex)
        {
            _logger.LogError(ex, "ElementExistsException");
            return ClientOperationResponse.BadRequest(ex.Message);
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return ClientOperationResponse.BadRequest($"Ошибка хранилища: {ex.InnerException?.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return ClientOperationResponse.InternalServerError(ex.Message);
        }
    }

    /// <summary>
    /// Удалить клиента
    /// </summary>
    public ClientOperationResponse Delete(string id)
    {
        try
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));

            _businessLogic.DeleteClient(id);
            return ClientOperationResponse.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return ClientOperationResponse.BadRequest("ID клиента пуст");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return ClientOperationResponse.BadRequest($"Некорректный ID: {ex.Message}");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return ClientOperationResponse.BadRequest($"Клиент с ID {id} не найден");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return ClientOperationResponse.BadRequest($"Ошибка хранилища: {ex.InnerException?.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return ClientOperationResponse.InternalServerError(ex.Message);
        }
    }
}