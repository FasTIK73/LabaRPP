using RPP.DataModels;

namespace RPP.BusinessLogicsContracts;

public interface IPostBusinessLogicContract
{
    List<PostDataModel> GetAllPosts();
    List<PostDataModel> GetAllDataOfPost(string postId);
    PostDataModel GetPostByData(string data);
    void InsertPost(PostDataModel model);
    void UpdatePost(PostDataModel model);
    void DeletePost(string id);
    void RestorePost(string id);
}