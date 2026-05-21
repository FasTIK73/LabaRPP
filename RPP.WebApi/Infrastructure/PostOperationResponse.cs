using RPP.WebApi.Models.ViewModels;

namespace RPP.WebApi.Infrastructure;

public class PostOperationResponse : OperationResponse
{
    public static PostOperationResponse OK(List<PostViewModel> data)
        => OK<PostOperationResponse, List<PostViewModel>>(data);

    public static PostOperationResponse OK(PostViewModel data)
        => OK<PostOperationResponse, PostViewModel>(data);

    public static PostOperationResponse NoContent()
        => NoContent<PostOperationResponse>();

    public static PostOperationResponse NotFound(string message)
        => NotFound<PostOperationResponse>(message);

    public static PostOperationResponse BadRequest(string message)
        => BadRequest<PostOperationResponse>(message);

    public static PostOperationResponse InternalServerError(string message)
        => InternalServerError<PostOperationResponse>(message);
}