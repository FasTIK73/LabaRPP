using RPP.DataModels;

namespace RPP.StoragesContracts;

public interface IProductStorageContract
{
    List<ProductDataModel> GetList(bool onlyActive = true, string? manufacturerId = null);
}