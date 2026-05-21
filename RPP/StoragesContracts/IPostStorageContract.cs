using RPP.DataModels;

namespace RPP.StoragesContracts;

public interface IPostStorageContract
{
    List<PostDataModel> GetList();
    List<PostDataModel> GetPostWithHistory(string postId);
    PostDataModel? GetElementById(string id);
    PostDataModel? GetElementByName(string name);
    void AddElement(PostDataModel element);
    void UpdElement(PostDataModel element);
    void DelElement(string id);
    void ResElement(string id);
}