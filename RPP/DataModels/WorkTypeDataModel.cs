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

public class WorkTypeDataModel : IValidation
{
    public string Id { get; private set; }
    public string WorkName { get; private set; }
    public MeasurementUnit Unit { get; private set; }
    public double PricePerUnit { get; private set; }

    // Историчность типа 4 - история изменения цены
    public double? PreviousPrice { get; private set; }
    public DateTime? PriceChangeDate { get; private set; }

    public WorkTypeDataModel(string id, string workName, MeasurementUnit unit,
        double pricePerUnit, double? previousPrice = null, DateTime? priceChangeDate = null)
    {
        Id = id;
        WorkName = workName;
        Unit = unit;
        PricePerUnit = pricePerUnit;
        PreviousPrice = previousPrice;
        PriceChangeDate = priceChangeDate;
    }

    public void Validate()
    {
        if (Id.IsEmpty())
            throw new ValidationException("Field Id is empty");

        if (!Id.IsGuid())
            throw new ValidationException("The value in the field Id is not a unique identifier");

        if (WorkName.IsEmpty())
            throw new ValidationException("Field WorkName is empty");

        if (Unit == MeasurementUnit.None)
            throw new ValidationException("Field Unit is empty");

        if (PricePerUnit <= 0)
            throw new ValidationException("Field PricePerUnit must be greater than 0");

        // Проверка историчности
        if (PreviousPrice.HasValue && PreviousPrice <= 0)
            throw new ValidationException("PreviousPrice must be greater than 0");

        if (PriceChangeDate.HasValue && PriceChangeDate > DateTime.Now)
            throw new ValidationException("PriceChangeDate cannot be in the future");
    }
}