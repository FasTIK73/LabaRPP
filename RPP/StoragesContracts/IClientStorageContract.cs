using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RPP.DataModels;

namespace RPP.StoragesContracts;

public interface IClientStorageContract
{
    List<ClientDataModel> GetList();
    ClientDataModel? GetElementById(string id);
    ClientDataModel? GetElementByPhone(string phoneNumber);
    ClientDataModel? GetElementByName(string name);
    void AddElement(ClientDataModel element);
    void UpdateElement(ClientDataModel element);
    void DeleteElement(string id);
}