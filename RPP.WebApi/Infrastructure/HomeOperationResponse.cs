using RPP.WebApi.Models.ViewModels;

namespace RPP.WebApi.Infrastructure;

public class HomeOperationResponse : OperationResponse
{
    public static HomeOperationResponse OK(List<HomeViewModel> data)
        => OK<HomeOperationResponse, List<HomeViewModel>>(data);

    public static HomeOperationResponse OK(HomeViewModel data)
        => OK<HomeOperationResponse, HomeViewModel>(data);

    public static HomeOperationResponse NoContent()
        => NoContent<HomeOperationResponse>();

    public static HomeOperationResponse NotFound(string message)
        => NotFound<HomeOperationResponse>(message);

    public static HomeOperationResponse BadRequest(string message)
        => BadRequest<HomeOperationResponse>(message);

    public static HomeOperationResponse InternalServerError(string message)
        => InternalServerError<HomeOperationResponse>(message);
}