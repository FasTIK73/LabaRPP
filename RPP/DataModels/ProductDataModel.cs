using RPP.Common.Enums;
using RPP.Common.Exceptions;
using RPP.Common.Extensions;
using RPP.Common.Infrastructure;

namespace RPP.DataModels;

public class ProductDataModel : IValidation
{
    public string Id { get; private set; }
    public string ProductName { get; private set; }
    public ProductType ProductType { get; private set; }
    public string ManufacturerId { get; private set; }
    public double Price { get; private set; }
    public bool IsDeleted { get; private set; }

    public ProductDataModel(string id, string productName, ProductType productType,
        string manufacturerId, double price, bool isDeleted)
    {
        Id = id;
        ProductName = productName;
        ProductType = productType;
        ManufacturerId = manufacturerId;
        Price = price;
        IsDeleted = isDeleted;
    }

    public void Validate()
    {
        if (Id.IsEmpty())
            throw new ValidationException("Field Id is empty");
        if (!Id.IsGuid())
            throw new ValidationException("Id is not a valid GUID");
        if (ProductName.IsEmpty())
            throw new ValidationException("Field ProductName is empty");
        if (ManufacturerId.IsEmpty())
            throw new ValidationException("Field ManufacturerId is empty");
        if (!ManufacturerId.IsGuid())
            throw new ValidationException("ManufacturerId is not a valid GUID");
        if (Price <= 0)
            throw new ValidationException("Price must be greater than 0");
    }
}