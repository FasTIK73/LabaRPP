using RPP.Common.Exceptions;
using RPP.Common.Extensions;
using RPP.Common.Infrastructure;

namespace RPP.DataModels;

public class ManufacturerDataModel : IValidation
{
    public string Id { get; private set; }
    public string ManufacturerName { get; private set; }
    public string? PrevManufacturerName { get; private set; }
    public string? PrevPrevManufacturerName { get; private set; }

    public ManufacturerDataModel(string id, string manufacturerName, string? prevManufacturerName, string? prevPrevManufacturerName)
    {
        Id = id;
        ManufacturerName = manufacturerName;
        PrevManufacturerName = prevManufacturerName;
        PrevPrevManufacturerName = prevPrevManufacturerName;
    }

    public void Validate()
    {
        if (Id.IsEmpty())
            throw new ValidationException("Field Id is empty");
        if (!Id.IsGuid())
            throw new ValidationException("Id is not a valid GUID");
        if (ManufacturerName.IsEmpty())
            throw new ValidationException("Field ManufacturerName is empty");
    }
}