using RPP.Common.Enums;
using RPP.Common.Exceptions;
using RPP.Common.Extensions;
using RPP.Common.Infrastructure;

namespace RPP.DataModels;

public class ReportDataModel : IValidation
{
    public string Id { get; private set; }
    public string HomeId { get; private set; }
    public string WorkTypeId { get; private set; }
    public string WorkerId { get; private set; }
    public string ToolId { get; private set; }
    public DateTime WorkDate { get; private set; }
    public double WorkVolume { get; private set; }
    public ReportStatus Status { get; private set; }
    public double TotalCost { get; private set; }

    public ReportDataModel(string id, string homeId, string workTypeId,
        string workerId, string toolId, DateTime workDate, double workVolume,
        ReportStatus status, double totalCost)
    {
        Id = id;
        HomeId = homeId;
        WorkTypeId = workTypeId;
        WorkerId = workerId;
        ToolId = toolId;
        WorkDate = workDate;
        WorkVolume = workVolume;
        Status = status;
        TotalCost = totalCost;
    }

    public void Validate()
    {
        if (Id.IsEmpty())
            throw new ValidationException("Field Id is empty");

        if (!Id.IsGuid())
            throw new ValidationException("The value in the field Id is not a unique identifier");

        if (HomeId.IsEmpty())
            throw new ValidationException("Field HomeId is empty");

        if (!HomeId.IsGuid())
            throw new ValidationException("The value in the field HomeId is not a unique identifier");

        if (WorkTypeId.IsEmpty())
            throw new ValidationException("Field WorkTypeId is empty");

        if (!WorkTypeId.IsGuid())
            throw new ValidationException("The value in the field WorkTypeId is not a unique identifier");

        if (WorkerId.IsEmpty())
            throw new ValidationException("Field WorkerId is empty");

        if (!WorkerId.IsGuid())
            throw new ValidationException("The value in the field WorkerId is not a unique identifier");

        if (ToolId.IsEmpty())
            throw new ValidationException("Field ToolId is empty");

        if (!ToolId.IsGuid())
            throw new ValidationException("The value in the field ToolId is not a unique identifier");

        if (WorkDate > DateTime.Now)
            throw new ValidationException("Work date cannot be in the future");

        if (WorkVolume <= 0)
            throw new ValidationException("WorkVolume must be greater than 0");

        if (Status == ReportStatus.None)
            throw new ValidationException("Field Status is empty");

        if (TotalCost <= 0)
            throw new ValidationException("TotalCost must be greater than 0");
    }
}