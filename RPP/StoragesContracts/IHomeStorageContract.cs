using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RPP.DataModels;
using RPP.Enums;

namespace RPP.StoragesContracts;

public interface IHomeStorageContract
{
    List<HomeDataModel> GetList();
    List<HomeDataModel> GetListByClient(string clientId);
    List<HomeDataModel> GetListByStatus(HomeStatus status);
    List<HomeDataModel> GetListByType(HomeType type);
    HomeDataModel? GetElementById(string id);
    HomeDataModel? GetElementByAddress(string address);
    void AddElement(HomeDataModel element);
    void UpdateElement(HomeDataModel element);
    void DeleteElement(string id);
}