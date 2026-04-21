using RPP.Common.Enums;
using RPP.Common.Exceptions;
using RPP.Common.Extensions;
using RPP.Common.Infrastructure;

namespace RPP.DataModels;

public class HomeDataModel : IValidation
{
    public string Id { get; private set; }
    public string ClientId { get; private set; }
    public string Address { get; private set; }
    public double Area { get; private set; }
    public HomeType Type { get; private set; }
    public HomeStatus Status { get; private set; }

    public HomeDataModel(string id, string clientId, string address,
        double area, HomeType type, HomeStatus status)
    {
        Id = id;
        ClientId = clientId;
        Address = address;
        Area = area;
        Type = type;
        Status = status;
    }

    public void Validate()
    {
        if (Id.IsEmpty())
            throw new ValidationException("Field Id is empty");

        if (!Id.IsGuid())
            throw new ValidationException("The value in the field Id is not a unique identifier");

        if (ClientId.IsEmpty())
            throw new ValidationException("Field ClientId is empty");

        if (!ClientId.IsGuid())
            throw new ValidationException("The value in the field ClientId is not a unique identifier");

        if (Address.IsEmpty())
            throw new ValidationException("Field Address is empty");

        if (Area <= 0)
            throw new ValidationException("Field Area must be greater than 0");

        if (Type == HomeType.None)
            throw new ValidationException("Field Type is empty");

        if (Status == HomeStatus.None)
            throw new ValidationException("Field Status is empty");
    }
}