using AutoMapper;
using RPP.BusinessLogicsContracts;
using RPP.Common.Exceptions;
using RPP.DataModels;
using RPP.WebApi.Infrastructure;
using RPP.WebApi.Models.BindingModels;
using RPP.WebApi.Models.ViewModels;

namespace RPP.WebApi.Adapters;

public class PostAdapter
{
    private readonly IPostBusinessLogicContract _businessLogic;
    private readonly IMapper _mapper;
    private readonly ILogger<PostAdapter> _logger;

    public PostAdapter(IPostBusinessLogicContract businessLogic, IMapper mapper, ILogger<PostAdapter> logger)
    {
        _businessLogic = businessLogic;
        _mapper = mapper;
        _logger = logger;
    }

    public PostOperationResponse GetAll()
    {
        try
        {
            var posts = _businessLogic.GetAllPosts();
            var viewModels = _mapper.Map<List<PostViewModel>>(posts);
            return PostOperationResponse.OK(viewModels);
        }
        catch (NullListException ex)
        {
            _logger.LogError(ex, "NullListException");
            return PostOperationResponse.NotFound("Список должностей не инициализирован");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return PostOperationResponse.InternalServerError($"Ошибка хранилища: {ex.InnerException?.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return PostOperationResponse.InternalServerError(ex.Message);
        }
    }

    public PostOperationResponse GetHistory(string id)
    {
        try
        {
            var history = _businessLogic.GetAllDataOfPost(id);
            var viewModels = _mapper.Map<List<PostViewModel>>(history);
            return PostOperationResponse.OK(viewModels);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return PostOperationResponse.BadRequest("ID должности пуст");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return PostOperationResponse.BadRequest($"Некорректный ID: {ex.Message}");
        }
        catch (NullListException ex)
        {
            _logger.LogError(ex, "NullListException");
            return PostOperationResponse.NotFound("История должности не найдена");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return PostOperationResponse.InternalServerError($"Ошибка хранилища: {ex.InnerException?.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return PostOperationResponse.InternalServerError(ex.Message);
        }
    }

    public PostOperationResponse GetByData(string data)
    {
        try
        {
            var post = _businessLogic.GetPostByData(data);
            var viewModel = _mapper.Map<PostViewModel>(post);
            return PostOperationResponse.OK(viewModel);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return PostOperationResponse.BadRequest("Данные для поиска пусты");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return PostOperationResponse.NotFound($"Должность с данными '{data}' не найдена");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return PostOperationResponse.InternalServerError($"Ошибка хранилища: {ex.InnerException?.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return PostOperationResponse.InternalServerError(ex.Message);
        }
    }

    public PostOperationResponse Create(PostBindingModel model)
    {
        try
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var dataModel = new PostDataModel(
                Guid.NewGuid().ToString(),
                model.PostName!,
                model.PostType,
                model.ConfigurationJson!
            );
            _businessLogic.InsertPost(dataModel);
            return PostOperationResponse.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return PostOperationResponse.BadRequest("Данные должности пусты");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return PostOperationResponse.BadRequest($"Некорректные данные: {ex.Message}");
        }
        catch (ElementExistsException ex)
        {
            _logger.LogError(ex, "ElementExistsException");
            return PostOperationResponse.BadRequest(ex.Message);
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return PostOperationResponse.BadRequest($"Ошибка хранилища: {ex.InnerException?.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return PostOperationResponse.InternalServerError(ex.Message);
        }
    }

    public PostOperationResponse Update(PostBindingModel model)
    {
        try
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (string.IsNullOrEmpty(model.Id))
                throw new ValidationException("ID должности обязателен для обновления");

            var dataModel = new PostDataModel(
                model.Id,
                model.PostName!,
                model.PostType,
                model.ConfigurationJson!
            );
            _businessLogic.UpdatePost(dataModel);
            return PostOperationResponse.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return PostOperationResponse.BadRequest("Данные должности пусты");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return PostOperationResponse.BadRequest($"Некорректные данные: {ex.Message}");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return PostOperationResponse.BadRequest($"Должность с ID {model.Id} не найдена");
        }
        catch (ElementExistsException ex)
        {
            _logger.LogError(ex, "ElementExistsException");
            return PostOperationResponse.BadRequest(ex.Message);
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return PostOperationResponse.BadRequest($"Ошибка хранилища: {ex.InnerException?.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return PostOperationResponse.InternalServerError(ex.Message);
        }
    }

    public PostOperationResponse Delete(string id)
    {
        try
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));

            _businessLogic.DeletePost(id);
            return PostOperationResponse.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return PostOperationResponse.BadRequest("ID должности пуст");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return PostOperationResponse.BadRequest($"Некорректный ID: {ex.Message}");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return PostOperationResponse.BadRequest($"Должность с ID {id} не найдена");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return PostOperationResponse.BadRequest($"Ошибка хранилища: {ex.InnerException?.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return PostOperationResponse.InternalServerError(ex.Message);
        }
    }

    public PostOperationResponse Restore(string id)
    {
        try
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));

            _businessLogic.RestorePost(id);
            return PostOperationResponse.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return PostOperationResponse.BadRequest("ID должности пуст");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return PostOperationResponse.BadRequest($"Некорректный ID: {ex.Message}");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return PostOperationResponse.BadRequest($"Должность с ID {id} не найдена");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return PostOperationResponse.BadRequest($"Ошибка хранилища: {ex.InnerException?.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return PostOperationResponse.InternalServerError(ex.Message);
        }
    }
}