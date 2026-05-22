using RPP.DataModels;

namespace RPP.StoragesContracts;

public interface IManufacturerStorageContract
{
    List<ManufacturerDataModel> GetList();
    ManufacturerDataModel? GetElementById(string id);
}