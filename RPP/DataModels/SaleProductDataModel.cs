using RPP.Common.Exceptions;
using RPP.Common.Extensions;
using RPP.Common.Infrastructure;

namespace RPP.DataModels;

public class SaleProductDataModel : IValidation
{
    public string SaleId { get; private set; }
    public string ProductId { get; private set; }
    public int Count { get; private set; }
    public double Price { get; private set; }

    public SaleProductDataModel(string saleId, string productId, int count, double price)
    {
        SaleId = saleId;
        ProductId = productId;
        Count = count;
        Price = price;
    }

    public void Validate()
    {
        if (SaleId.IsEmpty())
            throw new ValidationException("Field SaleId is empty");

        if (!SaleId.IsGuid())
            throw new ValidationException("The value in the field SaleId is not a unique identifier");

        if (ProductId.IsEmpty())
            throw new ValidationException("Field ProductId is empty");

        if (!ProductId.IsGuid())
            throw new ValidationException("The value in the field ProductId is not a unique identifier");

        if (Count <= 0)
            throw new ValidationException("Field Count is less than or equal to 0");

        if (Price <= 0)
            throw new ValidationException("Field Price is less than or equal to 0");
    }
}