using RPP.WebApi.Models.ViewModels;

namespace RPP.WebApi.Infrastructure;

public class ToolOperationResponse : OperationResponse
{
    public static ToolOperationResponse OK(List<ToolViewModel> data)
        => OK<ToolOperationResponse, List<ToolViewModel>>(data);

    public static ToolOperationResponse OK(ToolViewModel data)
        => OK<ToolOperationResponse, ToolViewModel>(data);

    public static ToolOperationResponse NoContent()
        => NoContent<ToolOperationResponse>();

    public static ToolOperationResponse NotFound(string message)
        => NotFound<ToolOperationResponse>(message);

    public static ToolOperationResponse BadRequest(string message)
        => BadRequest<ToolOperationResponse>(message);

    public static ToolOperationResponse InternalServerError(string message)
        => InternalServerError<ToolOperationResponse>(message);
}