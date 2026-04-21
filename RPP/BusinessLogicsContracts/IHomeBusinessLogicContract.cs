using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RPP.DataModels;
using RPP.Enums;

namespace RPP.BusinessLogicsContracts;

public interface IHomeBusinessLogicContract
{
    List<HomeDataModel> GetAllHomes();
    List<HomeDataModel> GetHomesByClient(string clientId);
    List<HomeDataModel> GetHomesByStatus(HomeStatus status);
    List<HomeDataModel> GetHomesByType(HomeType type);
    HomeDataModel GetHomeByData(string data);
    void InsertHome(HomeDataModel model);
    void UpdateHome(HomeDataModel model);
    void DeleteHome(string id);
}