using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RPP.DataModels;

namespace RPP.BusinessLogicsContracts;

public interface IClientBusinessLogicContract
{
    List<ClientDataModel> GetAllClients();
    ClientDataModel GetClientByData(string data);
    void InsertClient(ClientDataModel model);
    void UpdateClient(ClientDataModel model);
    void DeleteClient(string id);
}