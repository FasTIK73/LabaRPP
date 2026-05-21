using Microsoft.Extensions.Logging;
using RPP.BusinessLogicsContracts;
using RPP.DataModels;
using RPP.Common.Exceptions;
using RPP.Common.Extensions;
using RPP.StoragesContracts;

namespace RPP.Implementations;

public class PostBusinessLogicContract : IPostBusinessLogicContract
{
    private readonly IPostStorageContract _storage;
    private readonly ILogger<PostBusinessLogicContract> _logger;

    public PostBusinessLogicContract(IPostStorageContract storage, ILogger<PostBusinessLogicContract> logger)
    {
        _storage = storage;
        _logger = logger;
    }

    public List<PostDataModel> GetAllPosts()
    {
        _logger.LogInformation("GetAllPosts called");
        return _storage.GetList() ?? throw new NullListException();
    }

    public List<PostDataModel> GetAllDataOfPost(string postId)
    {
        _logger.LogInformation("GetAllDataOfPost called for {PostId}", postId);
        if (postId.IsEmpty())
            throw new ArgumentNullException(nameof(postId));
        if (!postId.IsGuid())
            throw new ValidationException("PostId is not a valid GUID");
        return _storage.GetPostWithHistory(postId) ?? throw new NullListException();
    }

    public PostDataModel GetPostByData(string data)
    {
        _logger.LogInformation("GetPostByData called with {Data}", data);
        if (data.IsEmpty())
            throw new ArgumentNullException(nameof(data));

        if (data.IsGuid())
            return _storage.GetElementById(data) ?? throw new ElementNotFoundException(data);

        return _storage.GetElementByName(data) ?? throw new ElementNotFoundException(data);
    }

    public void InsertPost(PostDataModel model)
    {
        _logger.LogInformation("InsertPost called");
        ArgumentNullException.ThrowIfNull(model);
        model.Validate();
        _storage.AddElement(model);
    }

    public void UpdatePost(PostDataModel model)
    {
        _logger.LogInformation("UpdatePost called");
        ArgumentNullException.ThrowIfNull(model);
        model.Validate();
        _storage.UpdElement(model);
    }

    public void DeletePost(string id)
    {
        _logger.LogInformation("DeletePost called for {Id}", id);
        if (id.IsEmpty())
            throw new ArgumentNullException(nameof(id));
        if (!id.IsGuid())
            throw new ValidationException("Id is not a valid GUID");
        _storage.DelElement(id);
    }

    public void RestorePost(string id)
    {
        _logger.LogInformation("RestorePost called for {Id}", id);
        if (id.IsEmpty())
            throw new ArgumentNullException(nameof(id));
        if (!id.IsGuid())
            throw new ValidationException("Id is not a valid GUID");
        _storage.ResElement(id);
    }
}