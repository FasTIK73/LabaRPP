using RPP.WebApi.Models.ViewModels;

namespace RPP.WebApi.Infrastructure;

public class ReportOperationResponse : OperationResponse
{
    public static ReportOperationResponse OK(List<ReportViewModel> data)
        => OK<ReportOperationResponse, List<ReportViewModel>>(data);

    public static ReportOperationResponse OK(ReportViewModel data)
        => OK<ReportOperationResponse, ReportViewModel>(data);

    public static ReportOperationResponse OK(object data)
        => OK<ReportOperationResponse, object>(data);

    public static ReportOperationResponse NoContent()
        => NoContent<ReportOperationResponse>();

    public static ReportOperationResponse NotFound(string message)
        => NotFound<ReportOperationResponse>(message);

    public static ReportOperationResponse BadRequest(string message)
        => BadRequest<ReportOperationResponse>(message);

    public static ReportOperationResponse InternalServerError(string message)
        => InternalServerError<ReportOperationResponse>(message);
}