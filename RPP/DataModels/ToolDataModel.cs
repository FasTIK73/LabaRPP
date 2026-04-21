using RPP.Common.Exceptions;
using RPP.Common.Extensions;
using RPP.Common.Infrastructure;

namespace RPP.DataModels;

public class ToolDataModel : IValidation
{
    public string Id { get; private set; }
    public string ToolName { get; private set; }
    public string Description { get; private set; }
    public bool IsAvailable { get; private set; }
    public string? PreviousToolName { get; private set; }

    public ToolDataModel(string id, string toolName, string description,
        bool isAvailable, string? previousToolName = null)
    {
        Id = id;
        ToolName = toolName;
        Description = description;
        IsAvailable = isAvailable;
        PreviousToolName = previousToolName;
    }

    public void Validate()
    {
        if (Id.IsEmpty())
            throw new ValidationException("Field Id is empty");

        if (!Id.IsGuid())
            throw new ValidationException("The value in the field Id is not a unique identifier");

        if (ToolName.IsEmpty())
            throw new ValidationException("Field ToolName is empty");

        if (ToolName.Length < 2)
            throw new ValidationException("ToolName is too short (minimum 2 characters)");
    }
}