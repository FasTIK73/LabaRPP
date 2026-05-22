using RPP.DataModels;

namespace RPP.StoragesContracts;

public interface IBuyerStorageContract
{
    List<BuyerDataModel> GetList();
    BuyerDataModel? GetElementById(string id);
    BuyerDataModel? GetElementByPhone(string phoneNumber);
    BuyerDataModel? GetElementByName(string name);
    void AddElement(BuyerDataModel element);
    void UpdateElement(BuyerDataModel element);
    void DeleteElement(string id);
}