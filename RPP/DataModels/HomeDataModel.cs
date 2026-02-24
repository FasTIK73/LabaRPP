using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using RPP.Enums;
using RPP.Exceptions;
using RPP.Extensions;
using RPP.Infrastructure;
using RPP.Enums;
using RPP.Extensions;
using RPP.Infrastructure;

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
        // Проверка Id
        if (Id.IsEmpty())
            throw new ValidationException("Field Id is empty");

        if (!Id.IsGuid())
            throw new ValidationException("The value in the field Id is not a unique identifier");

        // Проверка ClientId
        if (ClientId.IsEmpty())
            throw new ValidationException("Field ClientId is empty");

        if (!ClientId.IsGuid())
            throw new ValidationException("The value in the field ClientId is not a unique identifier");

        // Проверка адреса
        if (Address.IsEmpty())
            throw new ValidationException("Field Address is empty");

        // Проверка площади
        if (Area <= 0)
            throw new ValidationException("Field Area must be greater than 0");

        // Проверка типа
        if (Type == HomeType.None)
            throw new ValidationException("Field Type is empty");

        // Проверка статуса
        if (Status == HomeStatus.None)
            throw new ValidationException("Field Status is empty");
    }
}