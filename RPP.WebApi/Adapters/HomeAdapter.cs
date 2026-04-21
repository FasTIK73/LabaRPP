using AutoMapper;
using RPP.BusinessLogicsContracts;
using RPP.Common.Exceptions;
using RPP.DataModels;
using RPP.WebApi.Infrastructure;
using RPP.WebApi.Models.BindingModels;
using RPP.WebApi.Models.ViewModels;

namespace RPP.WebApi.Adapters;

/// <summary>
/// Адаптер для работы с помещениями
/// </summary>
public class HomeAdapter
{
    private readonly IHomeBusinessLogicContract _businessLogic;
    private readonly IMapper _mapper;
    private readonly ILogger<HomeAdapter> _logger;

    public HomeAdapter(
        IHomeBusinessLogicContract businessLogic,
        IMapper mapper,
        ILogger<HomeAdapter> logger)
    {
        _businessLogic = businessLogic;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Получить все помещения
    /// </summary>
    public HomeOperationResponse GetAll()
    {
        try
        {
            var homes = _businessLogic.GetAllHomes();
            var viewModels = _mapper.Map<List<HomeViewModel>>(homes);
            return HomeOperationResponse.OK(viewModels);
        }
        catch (NullListException ex)
        {
            _logger.LogError(ex, "NullListException");
            return HomeOperationResponse.NotFound("Список помещений не инициализирован");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return HomeOperationResponse.InternalServerError($"Ошибка хранилища: {ex.InnerException?.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return HomeOperationResponse.InternalServerError(ex.Message);
        }
    }

    /// <summary>
    /// Получить помещение по данным (ID или адрес)
    /// </summary>
    public HomeOperationResponse GetByData(string data)
    {
        try
        {
            var home = _businessLogic.GetHomeByData(data);
            var viewModel = _mapper.Map<HomeViewModel>(home);
            return HomeOperationResponse.OK(viewModel);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return HomeOperationResponse.BadRequest("Данные для поиска пусты");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return HomeOperationResponse.NotFound($"Помещение с данными '{data}' не найдено");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return HomeOperationResponse.InternalServerError($"Ошибка хранилища: {ex.InnerException?.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return HomeOperationResponse.InternalServerError(ex.Message);
        }
    }

    /// <summary>
    /// Создать новое помещение
    /// </summary>
    public HomeOperationResponse Create(HomeBindingModel model)
    {
        try
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var dataModel = _mapper.Map<HomeDataModel>(model);
            _businessLogic.InsertHome(dataModel);
            return HomeOperationResponse.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return HomeOperationResponse.BadRequest("Данные помещения пусты");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return HomeOperationResponse.BadRequest($"Некорректные данные: {ex.Message}");
        }
        catch (ElementExistsException ex)
        {
            _logger.LogError(ex, "ElementExistsException");
            return HomeOperationResponse.BadRequest(ex.Message);
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return HomeOperationResponse.BadRequest($"Ошибка хранилища: {ex.InnerException?.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return HomeOperationResponse.InternalServerError(ex.Message);
        }
    }

    /// <summary>
    /// Обновить данные помещения
    /// </summary>
    public HomeOperationResponse Update(HomeBindingModel model)
    {
        try
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (string.IsNullOrEmpty(model.Id))
                throw new ValidationException("ID помещения обязателен для обновления");

            var dataModel = _mapper.Map<HomeDataModel>(model);
            _businessLogic.UpdateHome(dataModel);
            return HomeOperationResponse.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return HomeOperationResponse.BadRequest("Данные помещения пусты");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return HomeOperationResponse.BadRequest($"Некорректные данные: {ex.Message}");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return HomeOperationResponse.BadRequest($"Помещение с ID {model.Id} не найдено");
        }
        catch (ElementExistsException ex)
        {
            _logger.LogError(ex, "ElementExistsException");
            return HomeOperationResponse.BadRequest(ex.Message);
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return HomeOperationResponse.BadRequest($"Ошибка хранилища: {ex.InnerException?.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return HomeOperationResponse.InternalServerError(ex.Message);
        }
    }

    /// <summary>
    /// Удалить помещение
    /// </summary>
    public HomeOperationResponse Delete(string id)
    {
        try
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));

            _businessLogic.DeleteHome(id);
            return HomeOperationResponse.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return HomeOperationResponse.BadRequest("ID помещения пуст");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return HomeOperationResponse.BadRequest($"Некорректный ID: {ex.Message}");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return HomeOperationResponse.BadRequest($"Помещение с ID {id} не найдено");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return HomeOperationResponse.BadRequest($"Ошибка хранилища: {ex.InnerException?.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return HomeOperationResponse.InternalServerError(ex.Message);
        }
    }
}