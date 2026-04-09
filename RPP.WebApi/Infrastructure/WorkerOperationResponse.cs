using RPP.WebApi.Models.ViewModels;

namespace RPP.WebApi.Infrastructure;

public class WorkerOperationResponse : OperationResponse
{
    public static WorkerOperationResponse OK(List<WorkerViewModel> data)
        => OK<WorkerOperationResponse, List<WorkerViewModel>>(data);

    public static WorkerOperationResponse OK(WorkerViewModel data)
        => OK<WorkerOperationResponse, WorkerViewModel>(data);

    public static WorkerOperationResponse NoContent()
        => NoContent<WorkerOperationResponse>();

    public static WorkerOperationResponse NotFound(string message)
        => NotFound<WorkerOperationResponse>(message);

    public static WorkerOperationResponse BadRequest(string message)
        => BadRequest<WorkerOperationResponse>(message);

    public static WorkerOperationResponse InternalServerError(string message)
        => InternalServerError<WorkerOperationResponse>(message);
}