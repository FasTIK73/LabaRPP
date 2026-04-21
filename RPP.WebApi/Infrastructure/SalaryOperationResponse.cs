using RPP.WebApi.Models.ViewModels;

namespace RPP.WebApi.Infrastructure;

public class SalaryOperationResponse : OperationResponse
{
    public static SalaryOperationResponse OK(SalaryViewModel data)
        => OK<SalaryOperationResponse, SalaryViewModel>(data);

    public static SalaryOperationResponse NoContent()
        => NoContent<SalaryOperationResponse>();

    public static SalaryOperationResponse NotFound(string message)
        => NotFound<SalaryOperationResponse>(message);

    public static SalaryOperationResponse BadRequest(string message)
        => BadRequest<SalaryOperationResponse>(message);

    public static SalaryOperationResponse InternalServerError(string message)
        => InternalServerError<SalaryOperationResponse>(message);
}