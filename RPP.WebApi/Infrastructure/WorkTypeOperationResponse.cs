using RPP.WebApi.Models.ViewModels;

namespace RPP.WebApi.Infrastructure;

public class WorkTypeOperationResponse : OperationResponse
{
    public static WorkTypeOperationResponse OK(List<WorkTypeViewModel> data)
        => OK<WorkTypeOperationResponse, List<WorkTypeViewModel>>(data);

    public static WorkTypeOperationResponse OK(WorkTypeViewModel data)
        => OK<WorkTypeOperationResponse, WorkTypeViewModel>(data);

    public static WorkTypeOperationResponse NoContent()
        => NoContent<WorkTypeOperationResponse>();

    public static WorkTypeOperationResponse NotFound(string message)
        => NotFound<WorkTypeOperationResponse>(message);

    public static WorkTypeOperationResponse BadRequest(string message)
        => BadRequest<WorkTypeOperationResponse>(message);

    public static WorkTypeOperationResponse InternalServerError(string message)
        => InternalServerError<WorkTypeOperationResponse>(message);
}